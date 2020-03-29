using System;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Core.Helpers;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Mappers.Import.Assets
{
    public class ImportAssetRackLookupResolver : IValueResolver<ImportAssetDto, AssetDto, Guid>
    {
        private readonly IRackRepository _rackRepo;
        private readonly IDatacenterRepository _datacenterRepo;

        public ImportAssetRackLookupResolver(IRackRepository rackRepo, IDatacenterRepository datacenterRepo)
        {
            _rackRepo = rackRepo;
            _datacenterRepo = datacenterRepo;
        }

        public Guid Resolve(ImportAssetDto source, AssetDto destination, Guid destMember, ResolutionContext context)
        {
            var datacenter = _datacenterRepo.GetDatacenter(source.DatacenterAbbreviation);
            if (datacenter == null) return default;

            var rack = _rackRepo.GetRack(source.GetRowLetter(), source.GetRackNumber(), datacenter.Id);
            return rack?.Id ?? default;
        }
    }
}