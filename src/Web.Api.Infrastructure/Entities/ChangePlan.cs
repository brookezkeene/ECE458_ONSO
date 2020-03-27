using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Web.Api.Infrastructure.Entities
{
    [Table("ChangePlans")]
    public class ChangePlan
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExecutedDate { get; set; }
        public Guid CreatedById {get; set; }
        public Guid DatacenterId { get; set; }
        public string DatacenterName { get; set; }
        public string Name { get; set; }

    }
}
