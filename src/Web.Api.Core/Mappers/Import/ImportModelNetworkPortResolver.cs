using System.Collections.Generic;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services;

namespace Web.Api.Core.Mappers.Import
{
    public class ImportModelNetworkPortResolver : IValueResolver<ImportModelDto, ModelDto, List<ModelNetworkPortDto>>
    {
        public List<ModelNetworkPortDto> Resolve(ImportModelDto source, ModelDto destination,
            List<ModelNetworkPortDto> destMember, ResolutionContext context)
        {
            var list = new List<ModelNetworkPortDto>();
            if (source.EthernetPorts.GetValueOrDefault() == 0) return list;

            for (var n = 1; n <= source.EthernetPorts && n <= 4; n++)
            {
                var srcMember = typeof(ImportModelDto).GetProperty($"NetworkPortName{n}")
                    ?.GetValue(source) as string;
                list.Add(new ModelNetworkPortDto
                {
                    Number = n,
                    Name = string.IsNullOrWhiteSpace(srcMember) ? n.ToString() : srcMember
                });
            }

            return list;
        }
    }
}