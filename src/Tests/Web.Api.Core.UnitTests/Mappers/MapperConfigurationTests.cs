using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Web.Api.Core.Mappers;
using Web.Api.Core.Mappers.Import;
using Web.Api.Core.Mappers.Import.Assets;
using Web.Api.Core.Mappers.Import.Models;
using Web.Api.Core.UnitTests.Common;
using Web.Api.Mappers;
using Xunit;

namespace Web.Api.Core.UnitTests.Mappers
{
    public class MapperConfigurationTests : IClassFixture<BaseMapperFixture>
    {
        private readonly IMapper _mapper;

        public MapperConfigurationTests(BaseMapperFixture fixture)
        {
            _mapper = fixture.GetMapper();
        }

        [Fact]
        public void GlobalConfigurationIsValid()
        {
            _mapper.AssertConfigurationIsValid();
        }

        [Fact]
        public void ModelApiConfigurationIsValid()
        {
            _mapper.AssertConfigurationIsValid<ModelApiMapperProfile>();
        }

        [Fact]
        public void AssetApiConfigurationIsValid()
        {
            _mapper.AssertConfigurationIsValid<AssetApiMapperProfile>();
        }

        [Fact]
        public void RackApiConfigurationIsValid()
        {
            _mapper.AssertConfigurationIsValid<RackApiMapperProfile>();
        }

        [Fact]
        public void DatacenterApiConfigurationIsValid()
        {
            _mapper.AssertConfigurationIsValid<DatacenterApiMapperProfile>();
        }

        [Fact]
        public void NetworkConnectionApiConfigurationIsValid()
        {
            _mapper.AssertConfigurationIsValid<NetworkConnectionApiMapperProfile>();
        }

        [Fact]
        public void AuditLogApiConfigurationIsValid()
        {
            _mapper.AssertConfigurationIsValid<AuditLogApiMapperProfile>();
        }

        [Fact]
        public void ExportApiConfigurationIsValid()
        {
            _mapper.AssertConfigurationIsValid<ExportApiMapperProfile>();
        }

        [Fact]
        public void PowerConnectionApiConfigurationIsValid()
        {
            _mapper.AssertConfigurationIsValid<PowerConnectionApiMapperProfile>();
        }

        [Fact]
        public void ChangePlanApiConfigurationIsValid()
        {
            _mapper.AssertConfigurationIsValid<ChangePlanApiMapperProfile>();
        }

        [Fact]
        public void ModelImportConfigurationIsValid()
        {
            _mapper.AssertConfigurationIsValid<ModelImportMapperProfile>();
        }

        [Fact]
        public void AssetImportConfigurationIsValid()
        {
            _mapper.AssertConfigurationIsValid<AssetImportMapperProfile>();
        }

        [Fact]
        public void NetworkConnectionImportConfigurationIsValid()
        {
            _mapper.AssertConfigurationIsValid<NetworkConnectionImportMapperProfile>();
        }

        [Fact]
        public void AuditLogConfigurationIsValid()
        {
            _mapper.AssertConfigurationIsValid<AuditLogMapperProfile>();
        }

        [Fact]
        public void DomainConfigurationIsValid()
        {
            _mapper.AssertConfigurationIsValid<DomainMapperProfile>();
        }
    }
}
