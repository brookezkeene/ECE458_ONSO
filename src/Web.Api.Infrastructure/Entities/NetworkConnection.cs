using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Infrastructure.Entities
{
    public class NetworkConnection
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public virtual List<AssetNetworkPort> Ports { get; set; }
    }
}