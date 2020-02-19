using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Infrastructure.Entities
{
    public class PduPort
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Range(1, 24)]
        public int Number { get; set; }

        public Guid PduId { get; set; }
        public virtual Pdu Pdu { get; set; }

        public Guid? AssetPowerPortId { get; set; }
        public virtual AssetPowerPort AssetPowerPort { get; set; }
    }
}