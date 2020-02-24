using System;

namespace Web.Api.Dtos.Models.Update
{
    public class UpdateModelNetworkPortDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
    }
}