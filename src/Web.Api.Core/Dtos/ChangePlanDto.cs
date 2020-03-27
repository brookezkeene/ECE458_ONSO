using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dtos
{
    public class ChangePlanDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExecutedDate { get; set; }
        public Guid CreatedById { get; set; }
        public Guid DatacenterId { get; set; }
        public string Name { get; set; }
    }
}
