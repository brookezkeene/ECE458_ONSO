using System.Collections.Generic;

namespace Web.Api.Dtos.Models.Read
{
    public class GetModelApiDto : GetModelsApiDto
    {
        public List<GetModelAssetApiDto> Assets { get; set; }
        public string Comment { get; set; }
        public List<GetModelNetworkPort> NetworkPorts { get; set; }
    }

}
