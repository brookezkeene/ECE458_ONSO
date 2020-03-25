using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dtos
{
    public class ChangePlanDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExecutedData { get; set; }
        public Guid CreatedById { get; set; }
    }
}
