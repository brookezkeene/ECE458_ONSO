using System;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Mappers.Import.Assets
{
    public class ImportAssetLookupResolver : IValueResolver<ImportAssetDto, AssetDto, Guid>
    {
        private readonly IAssetRepository _repo;

        public ImportAssetLookupResolver(IAssetRepository repo)
        {
            _repo = repo;
        }

        public Guid Resolve(ImportAssetDto source, AssetDto destination, Guid destMember, ResolutionContext context)
        {
            return source.AssetNumber.HasValue // if an asset number is provided
                ? _repo.GetAsset(source.AssetNumber.Value)?.Id ?? default // use it to lookup an asset and return its id if found, or the default if not found
                : default; // otherwise just return the default
        }
    }
}