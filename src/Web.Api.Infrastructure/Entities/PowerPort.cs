using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Infrastructure.Entities
{
    public abstract class PowerPort
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int Number { get; set; }
        public Guid? PowerConnectionId { get; set; }
        public virtual PowerConnection PowerConnection { get; set; }
    }
}