using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Web.Api.Infrastructure.Entities
{
    [Table("DecommissionedAssets")]
    public class DecommissionedAsset
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string OtherColumn { get; set; }
    }
}
