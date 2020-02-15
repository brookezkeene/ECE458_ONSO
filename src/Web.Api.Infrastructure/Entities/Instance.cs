using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Infrastructure.Entities
{
    public class Instance
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Model Model { get; set; }

        [Required]
        [MaxLength(63)]
        public string Hostname { get; set; }

        [Required]
        public Rack Rack { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int RackPosition { get; set; }

        public User Owner { get; set; }

        public string Comment { get; set; }
    }
}