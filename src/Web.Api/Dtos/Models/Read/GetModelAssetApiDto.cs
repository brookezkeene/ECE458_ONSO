using System;

namespace Web.Api.Dtos.Models.Read
{
    public class GetModelAssetApiDto
    {
        public Guid Id { get; set; }
        public string Hostname { get; set; } 
        public int AssetNumber { get; set; }
    }
}