using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Dtos.ChangePlans
{
    public class GetChangePlanApiDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExecutedData { get; set; }
        public Guid CreatedById { get; set; }
        public string Name { get; set; }
        public Guid DatacenterId { get; set; }
    }
}
