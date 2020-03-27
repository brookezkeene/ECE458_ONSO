using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Dtos.ChangePlans
{
    public class CreateChangePlanItemApiDto
    {
        public Guid ChangePlanId { get; set; }
        public Guid AssetId { get; set; }
        public string PreviousData { get; set; }
        public string NewData { get; set; }
    }
}
