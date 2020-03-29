using System;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Mappers.Import.Assets
{
    public class ImportAssetModelLookupResolver : IValueResolver<ImportAssetDto, AssetDto, Guid>
    {
        private readonly IModelRepository _repo;

        public ImportAssetModelLookupResolver(IModelRepository repo)
        {
            _repo = repo;
        }

        public Guid Resolve(ImportAssetDto source, AssetDto destination, Guid destMember, ResolutionContext context)
        {
            var model = _repo.GetModel(source.Vendor, source.ModelNumber);
            return model?.Id ?? default;
        }
    }
}