using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Web.Api.Infrastructure.Entities
{
    public class Datacenter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(6)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public bool HasNetworkManagedPower { get; set; }

        public virtual List<Rack> Racks { get; set; }
    }
}
