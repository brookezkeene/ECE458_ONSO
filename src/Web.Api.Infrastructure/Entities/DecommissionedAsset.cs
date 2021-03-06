﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Web.Api.Infrastructure.Entities
{
    public class DecommissionedAsset
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Hostname { get; set; }
        public string Datacenter { get; set; }
        public string ModelName { get; set; }
        public string ModelNumber { get; set; }
        public string Decommissioner { get; set; }
        public int RackPosition { get; set; }
        public string OwnerName { get; set; }
        public DateTime DateDecommissioned { get; set; }
        public string Rack { get; set; }
        public string Data { get; set; }
    }
}
