using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Infrastructure.Entities
{
    public class AssetNetworkPort
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [MaxLength(10)]
        [Required]
        public string Name { get; set; }

        [StringLength(17, MinimumLength = 17)]
        public string MacAddress { get; set; }

        public Guid AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        public Guid? ConnectedPortId { get; set; }
        public virtual AssetNetworkPort ConnectedPort { get; set; }
    }
}