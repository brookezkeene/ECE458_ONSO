using System;
using System.Collections.Generic;

namespace Web.Api.Core.Dtos
{
    public class ImportValidationDto
    {
        public Guid Id { get; set; }
        public List<string> Errors { get; set; }
        public List<string> Warnings { get; set; }
    }
}