using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Infrastructure.Entities
{
    public class PduPort : PowerPort
    {
        public Guid PduId { get; set; }
        public virtual Pdu Pdu { get; set; }
    }
}