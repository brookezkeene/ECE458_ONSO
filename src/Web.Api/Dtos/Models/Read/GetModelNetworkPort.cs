﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Dtos.Models.Read
{
    public class GetModelNetworkPort
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
    }
}
