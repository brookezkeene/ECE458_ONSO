using System.Collections.Generic;

namespace Web.Api.Dtos.Models.Read
{
    public class GetModelApiDto : GetModelsApiDto
    {
        public List<GetModelAssetApiDto> Assets;
        public string Comment { get; set; }
        public List<GetModelNetworkPort> NetworkPorts;
    }

}
