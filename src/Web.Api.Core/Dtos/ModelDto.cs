using System.Collections.Generic;

namespace Web.Api.Core.Dtos
{
    public class ModelDto : FlatModelDto
    {
        public List<FlatInstanceDto> Instances { get; set; }
    }
}