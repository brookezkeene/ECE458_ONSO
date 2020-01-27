using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Web.Api.Infrastructure
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

        public string Comment { get; set; }

        public List<Instance> Instances { get; set; }
    }

    public class Instance
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Model Model { get; set; }

        [Required]
        [MaxLength(255)]
        public string Hostname { get; set; }

        [Required]
        public Rack Rack { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int RackPosition { get; set; }

        public User Owner { get; set; }

        public string Comment { get; set; }
    }

    public class Rack
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Row { get; set; }

        [Required]
        public int Column { get; set; }

        public List<Instance> Instances { get; set; }
    }

    public class User : IdentityUser
    {
        [Required]
        public string DisplayName { get; set; }
    }
}
