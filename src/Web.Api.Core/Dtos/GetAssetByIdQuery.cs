using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dtos
{
    public class GetAssetByIdQuery
    {
        public Guid AssetId { get; set; }
        public Guid? ChangePlanId { get; set; }
    }
}
