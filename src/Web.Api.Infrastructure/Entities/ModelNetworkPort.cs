using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Infrastructure.Entities
{
    public class ModelNetworkPort
    {
        public Guid Id { get; set; }
        [MaxLength(10)]
        [Required]
        public string Name { get; set; }
        public Guid ModelId { get; set; }
        public virtual Model Model { get; set; }
    }
}