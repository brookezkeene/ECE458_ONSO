using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Infrastructure;

namespace Web.Api.Core
{
    public interface IModelService
    {
        Task<ModelsDto> GetModelsAsync(string search, int page = 1, int pageSize = 10);
        Task<ModelDto> GetModelAsync(Guid modelId);
    }

    public class ModelService : IModelService
    {
        private readonly IModelRepository _modelRepository;

        public ModelService(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public async Task<ModelsDto> GetModelsAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await _modelRepository.GetModelsAsync(search, page, pageSize);
            return pagedList.ToModel(); // note: ToModel refers to the concept of a model (as opposed to an entity), not the entity "Model"
        }

        public async Task<ModelDto> GetModelAsync(Guid modelId)
        {
            var model = await _modelRepository.GetModelAsync(modelId);
            // TODO: handle null result (no model found)
            return model.ToModel();
        }
    }

    public class ModelsDto : PagedList<ModelDto>
    {
    }

    public class ModelDto
    {
        public ModelDto()
        {
            Instances = new List<string>();
        }

        public Guid Id { get; set; }
        public string Vendor { get; set; }
        public string ModelNumber { get; set; }
        public int Height { get; set; }
        public string DisplayColor { get; set; }
        public int? EthernetPorts { get; set; }
        public int? PowerPorts { get; set; }
        public string Cpu { get; set; }
        public int? Memory { get; set; }
        public string Comment { get; set; }
        public List<string> Instances { get; set; }
    }
}
