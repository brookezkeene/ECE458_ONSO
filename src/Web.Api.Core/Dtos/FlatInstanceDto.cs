using System;

namespace Web.Api.Core.Dtos
{
    public class FlatInstanceDto
    {
        public Guid Id { get; set; }
        public string Hostname { get; set; }
        public int RackPosition { get; set; }
        public string Comment { get; set; }
    }
}