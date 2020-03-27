using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Dtos.ChangePlans
{
    public class CreateChangePlanApiDto
    {
        public Guid CreatedById { get; set; }
        public string Name { get; set; }
        public Guid DatacenterId { get; set; }
    }

}
