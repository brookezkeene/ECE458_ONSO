﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dtos
{
    public class DecommissionedAssetQuery
    {
        public Guid ChangePlanId { get; set; }
        public string NetworkPortGraph { get; set; }
        public Guid Id { get; set; }
        public string Decommissioner { get; set; }
    }

}
