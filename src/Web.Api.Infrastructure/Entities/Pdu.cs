using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Infrastructure.Entities
{
    public class Pdu
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [DefaultValue(24)]
        public int NumPorts { get; set; }

        public virtual List<PduPort> Ports { get; set; }

        public PduLocation Location { get; set; }

        public Guid RackId { get; set; }
        public virtual Rack Rack { get; set; }
    }
}