using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Dtos
{
    public class CreateAssetApiDto
    {
        public string Rack { get; set; }
        public string Hostname { get; set; }
        public string Comment { get; set; }
        public int RackPosition { get; set; }
        public string Owner { get; set; }
        public Guid Model { get; set; }
    }

}
