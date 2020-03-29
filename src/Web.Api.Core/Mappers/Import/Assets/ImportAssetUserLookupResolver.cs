using System;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Mappers.Import.Assets
{
    public class ImportAssetUserLookupResolver : IValueResolver<ImportAssetDto, AssetDto, Guid?>
    {
        private readonly IIdentityRepository _repo;

        public ImportAssetUserLookupResolver(IIdentityRepository repo)
        {
            _repo = repo;
        }

        public Guid? Resolve(ImportAssetDto source, AssetDto destination, Guid? destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.OwnerUsername)) return null;

            var user = _repo.GetUser(source.OwnerUsername);
            return user == null ? Guid.NewGuid() : Guid.Parse(user.Id);
        }
    }
}