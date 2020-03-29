using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper;
using Web.Api.Core.Dtos;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services
{
    public class AssetImportService : IAssetImportService
    {
        private readonly IImportRepository _importRepository;
        private readonly IMapper _mapper;
        private readonly IAssetService _assetService;

        public AssetImportService(IImportRepository importRepository, IMapper mapper, IAssetService assetService)
        {
            _importRepository = importRepository;
            _mapper = mapper;
            _assetService = assetService;
        }

        public async Task<ImportValidationDto> HandleImport(Stream stream)
        {
            var data = await ReadStream(stream);
            var id = await PersistData(data);
            var validationResult = Validate(data);

            validationResult.Id = id;

            return validationResult;
        }

        public async Task<BulkImportResultDto> FinalizeImport(Guid id)
        {
            var importFile = await _importRepository.GetImportAsync(id);
            var assets = ParseImport(importFile.Data);

            var added = 0;
            var updated = 0;
            foreach (var asset in assets)
            {
                if (asset.Id == default)
                {
                    await _assetService.CreateAssetAsync(asset);
                    added++;
                }
                else
                {
                    await _assetService.UpdateAssetAsync(asset);
                    updated++;
                }
            }

            return new BulkImportResultDto(added, updated);
        }

        private async Task<string> ReadStream(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        private ImportValidationDto Validate(string data)
        {
            // always pass validation
            return new ImportValidationDto
            {
                Errors = new List<string>(),
                Warnings = new List<string>()
            };
        }

        private IEnumerable<AssetDto> ParseImport(string data)
        {
            using var reader = new StringReader(data);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<ImportAssetDto>();

            return _mapper.Map<IEnumerable<AssetDto>>(records);
        }

        private async Task<Guid> PersistData(string data)
        {
            // returns id reference to this import for continuation
            return await _importRepository.AddImportAsync(data);
        }
    }
}