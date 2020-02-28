using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Infrastructure.Entities
{
    public class AssetPowerPort
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int Number { get; set; }

        public Guid AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        public Guid? PduPortId { get; set; }
        public virtual PduPort PduPort { get; set; }
    }
}