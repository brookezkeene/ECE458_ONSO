using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Dtos
{
    public class GetAuditEventApiDto
    {
        public long Id { get; set; }
        public string Event { get; set; }
        public string Source { get; set; }
        public string Category { get; set; }
        public string SubjectIdentifier { get; set; }
        public string SubjectName { get; set; }
        public string SubjectType { get; set; }
        public string Action { get; set; }
        public string Data { get; set; }
        public DateTime Created { get; set; }
    }
}
