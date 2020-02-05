using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IModelValidationService
    {
        Task<ValidationResult> Validate(FlatModelDto model);
    }
}
