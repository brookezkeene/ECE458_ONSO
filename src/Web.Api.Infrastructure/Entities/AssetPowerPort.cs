using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Infrastructure.Entities
{
    public class AssetPowerPort : PowerPort
    {
        public Guid AssetId { get; set; }
        public virtual Asset Asset { get; set; }
    }
}