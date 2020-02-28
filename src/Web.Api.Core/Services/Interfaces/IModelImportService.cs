using System;
using System.IO;
using System.Threading.Tasks;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IModelImportService
    {
        Task<ImportValidationDto> HandleImport(Stream stream);
        Task<BulkImportResultDto> FinalizeImport(Guid id);
    }
}