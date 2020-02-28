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
    public class ModelImportService : IModelImportService
    {
        private readonly IImportRepository _importRepository;
        private readonly IMapper _mapper;
        private readonly IModelService _modelService;

        public ModelImportService(IImportRepository importRepository, IMapper mapper, IModelService modelService)
        {
            _importRepository = importRepository;
            _mapper = mapper;
            _modelService = modelService;
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
            var models = ParseImport(importFile.Data);

            var added = 0;
            var updated = 0;
            foreach (var model in models)
            {
                if (model.Id == default)
                {
                    await _modelService.CreateModelAsync(model);
                    added++;
                }
                else
                {
                    await _modelService.UpdateModelAsync(model);
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

        private IEnumerable<ModelDto> ParseImport(string data)
        {
            using var reader = new StringReader(data);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<ImportModelDto>();

            return _mapper.Map<IEnumerable<ModelDto>>(records);
        }

        private async Task<Guid> PersistData(string data)
        {
            // returns id reference to this import for continuation
            return await _importRepository.AddImportAsync(data);
        }
    }
}