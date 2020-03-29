using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Web.Api.Infrastructure.Entities
{
    public class ChangePlanItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid ChangePlanId { get; set; }
        public Guid AssetId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ExecutionType { get; set; }
        public string PreviousData { get; set; }
        public string NewData { get; set; }
    }
}
