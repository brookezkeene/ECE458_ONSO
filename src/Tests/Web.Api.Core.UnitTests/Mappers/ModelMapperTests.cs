using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Web.Api.Common;
using Web.Api.Core.Mappers;
using Web.Api.Infrastructure.Entities;
using Xunit;

namespace Web.Api.Core.UnitTests.Mappers
{
    public class ModelMapperTests
    {
        [Fact]
        public void CanMapEntityToDto()
        {
            var model = ModelMock.GenerateRandomModel();

            var modelDto = model.ToModel();

            modelDto.Should().NotBeNull();
        }

        [Fact]
        public void CanMapListOfEntityToListOfDto()
        {
            var list = new PagedList<Model>();
            for (var i = 0; i < 5; i++)
            {
                list.Add(ModelMock.GenerateRandomModel());
            }

            var listDto = list.ToModel();

            listDto.Should()
                .NotBeNull();
        }
    }
}
