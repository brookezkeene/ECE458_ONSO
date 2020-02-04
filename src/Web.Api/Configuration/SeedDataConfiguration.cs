using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Configuration
{
    public class SeedDataConfiguration
    {
        public List<Rack> Racks { get; set; } = new List<Rack>();
        public List<Model> Models { get; set; } = new List<Model>();
        public List<Instance> Instances { get; set; } = new List<Instance>();
    }
}
