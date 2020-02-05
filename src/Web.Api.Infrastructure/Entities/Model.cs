using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Infrastructure.Entities
{
    public class Model
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Vendor { get; set; }

        [Required]
        [MaxLength(50)]
        public string ModelNumber { get; set; }

        [Required]
        [Range(1, int.MaxValue)] // positive number
        public int Height { get; set; }

        [MinLength(6)]
        [MaxLength(7)] // '#' is optional before hex color code
        public string DisplayColor { get; set; }

        public int? EthernetPorts { get; set; }

        public int? PowerPorts { get; set; }

        [MaxLength(50)]
        public string Cpu { get; set; }

        public int? Memory { get; set; }
        [MaxLength(50)]
        public string Storage { get; set; }

        public string Comment { get; set; }

        public List<Instance> Instances { get; set; }
    }
}