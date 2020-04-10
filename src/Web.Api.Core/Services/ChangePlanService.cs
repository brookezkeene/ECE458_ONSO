using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services
{
    
    public class ChangePlanService : IChangePlanService
    {
        private readonly IChangePlanRepository _repository;
        private readonly IAssetRepository _assetRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IRackRepository _rackRepository;
        private readonly IIdentityRepository _userRepository;
        private readonly IMapper _mapper;

        public ChangePlanService(IChangePlanRepository repository, IAssetRepository assetRepository, IModelRepository modelRepository, 
                                IRackRepository rackRepository, IIdentityRepository userRepository, IMapper mapper)
        {
            
            _repository = repository;
            _assetRepository = assetRepository;
            _modelRepository = modelRepository;
            _rackRepository = rackRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ChangePlanDto> GetChangePlanAsync(Guid changPlanId)
        {
            var changePlan = await _repository.GetChangePlanAsync(changPlanId);
            return _mapper.Map<ChangePlanDto>(changePlan);
        }

        public async Task<ChangePlanItemDto> GetChangePlanItemAsync(Guid changePlanItemId)
        {
            var changePlanItem = await _repository.GetChangePlanItemAsync(changePlanItemId);

            return _mapper.Map<ChangePlanItemDto>(changePlanItem);
        }
        public async Task<ChangePlanItemDto> GetChangePlanItemAsync(Guid changePlanId, Guid assetId)
        {
            var changePlanItem = await _repository.GetChangePlanItemAsync(changePlanId, assetId);
            return _mapper.Map<ChangePlanItemDto>(changePlanItem);
        }
        public async Task<PagedList<ChangePlanDto>> GetChangePlansAsync(Guid? createdById, int page = 1, int pageSize = 10)
        {
            var list = await _repository.GetChangePlansAsync(createdById, page, pageSize);
            return _mapper.Map<PagedList<ChangePlanDto>>(list);
        }

        public async Task<List<ChangePlanItemDto>> GetChangePlanItemsAsync(Guid changePlanId)
        {
            var list = await _repository.GetChangePlanItemsAsync(changePlanId);
            return _mapper.Map<List<ChangePlanItemDto>>(list);
        }
        public async Task<List<ChangePlanItemDto>> GetDecommissionedChangePlanItemsAsync(Guid changePlanId)
        {
            var list = await _repository.GetDecommissionedChangePlanItemsAsync(changePlanId);
            return _mapper.Map<List<ChangePlanItemDto>>(list);
        }
        public async Task<List<ChangePlanItemDto>> GetAsssetChangePlanItemsAsync(Guid changePlanId)
        {
            var list = await _repository.GetAssetChangePlanItemsAsync(changePlanId);
            return _mapper.Map<List<ChangePlanItemDto>>(list);
        }
        public async Task<Guid> CreateChangePlanAsync(ChangePlanDto changePlan)
        {
            var entity = _mapper.Map<ChangePlan>(changePlan);
            await _repository.AddChangePlanAsync(entity);
            return entity.Id;
        }

        public async Task<Guid> CreateChangePlanItemAsync(ChangePlanItemDto changePlanItem)
        {
            var entity = _mapper.Map<ChangePlanItem>(changePlanItem);
            await _repository.AddChangePlanItemAsync(entity);
            return entity.Id;
        }
        public async Task<Guid> CreateChangePlanItemAsync(Guid changePlanId, Guid assetId, DecommissionedAssetDto decommissionedAsset)
        {
            var changePlanItemDto = new ChangePlanItemDto
            {
                ChangePlanId = changePlanId,
                ExecutionType = "decommission",
                AssetId = assetId,
                NewData = JsonConvert.SerializeObject(decommissionedAsset),
                PreviousData = decommissionedAsset.Data,
                CreatedDate = DateTime.Now
            };
            var entity = _mapper.Map<ChangePlanItem>(changePlanItemDto);
            await _repository.AddChangePlanItemAsync(entity);
            return entity.Id;
        }
        public async Task<int> UpdateChangePlanItemAsync(ChangePlanItemDto changePlanItem)
        {
            var entity = _mapper.Map<ChangePlanItem>(changePlanItem);
            var updated = await _repository.UpdateChangePlanItemAsync(entity);
            return updated;
        }

        public async Task<int> UpdateChangePlanAsync(ChangePlanDto changePlan)
        {
            var entity = _mapper.Map<ChangePlan>(changePlan);
            var updated = await _repository.UpdateChangePlanAsync(entity);
            return updated;
        }

        public async Task DeleteChangePlanAsync(Guid changePlanId)
        {
            var entity = await _repository.GetChangePlanAsync(changePlanId);
            await _repository.DeleteChangePlanAsync(entity);
        }

        public async Task DeleteChangePlanItemAsync(Guid changePlanItemId)
        {
            var entity = await _repository.GetChangePlanItemAsync(changePlanItemId);
            await _repository.DeleteChangePlanItemAsync(entity);
        }

        public async Task<int> ExecuteChangePlan(List<ChangePlanItemDto> changePlanItems)
        {
            var changePlanItemsEntities = _mapper.Map<List<ChangePlanItem>>(changePlanItems);
            return await _repository.ExecuteChangePlan(changePlanItemsEntities);
        }
        public async Task<AssetDto> FillFieldsInAssetApiForChangePlans(AssetDto assetDto)
        {
            assetDto.Model = _mapper.Map<ModelDto>(await _modelRepository.GetModelAsync(assetDto.ModelId));
            assetDto.Rack = _mapper.Map<RackDto>(await _rackRepository.GetRackAsync(assetDto.RackId));
            if (assetDto.OwnerId != null || assetDto.OwnerId != Guid.Empty)
            {
                assetDto.Owner = _mapper.Map<UserDto>(await _userRepository.GetUserAsync(assetDto.OwnerId ?? Guid.Empty));
            }
            foreach(AssetNetworkPortDto assetNetworkPortDto in assetDto.NetworkPorts)
            {
                var networkPortModelId = assetNetworkPortDto.ModelNetworkPortId;
                assetNetworkPortDto.ModelNetworkPort = assetDto.Model.NetworkPorts.Find(x => x.Id == networkPortModelId);
                if (assetNetworkPortDto.ConnectedPortId != null)
                {
                    var connectedPort = await _assetRepository.GetNetworkPortAsync(assetNetworkPortDto.ConnectedPortId ?? Guid.Empty);
                    assetNetworkPortDto.ConnectedPort = _mapper.Map<AssetNetworkPortDto>(connectedPort);
                }
            }
            int count = 1;
            foreach (AssetPowerPortDto assetPowerPortDto in assetDto.PowerPorts)
            {
                assetPowerPortDto.Number = count;
                if (assetPowerPortDto.PduPortId != null)
                {
                    var connectedPort = await _assetRepository.GetPowerPortAsync(assetPowerPortDto.PduPortId ?? Guid.Empty);
                    assetPowerPortDto.PduPort = _mapper.Map<PduPortDto>(connectedPort);
                }
                count++;
            }
            return assetDto;
        }
    }
}
