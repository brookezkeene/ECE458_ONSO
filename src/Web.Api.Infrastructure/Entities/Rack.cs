using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Infrastructure.Entities
{
    public class Rack
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Row { get; set; }

        [Required]
        public int Column { get; set; }

        public virtual List<Asset> Assets { get; set; }

        public Guid? DatacenterId { get; set; }
        public virtual Datacenter Datacenter { get; set; }

        public virtual List<Pdu> Pdus { get; set; }
    }
}