﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Dtos.ChangePlans
{
    public class UpdateChangePlanItemApiDto
    {
        public Guid ChangePlanId { get; set; }
        public Guid AssetId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ExecutionType { get; set; }
        public string PreviousData { get; set; }
        public string NewData { get; set; }
    }
}
