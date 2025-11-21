using AutoMapper;
using LW_4_3_5_Daryev_PI231.Models;
using LW_4_3_5_Daryev_PI231.Repositories;
using LW_4_3_5_Daryev_PI231.DTOs;
using LW_4_3_5_Daryev_PI231.Enumerations;


namespace LW_4_3_5_Daryev_PI231.Services
{
    public class GamingAssetService : IGamingAssetService
    {
        private readonly IGamingAssetRepository _repository;
        private readonly IMapper _mapper;

        public GamingAssetService(IGamingAssetRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AssetDTO>> GetAllAssetsAsync()
        {
            var assets = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<AssetDTO>>(assets);
        }

        public async Task<AssetDTO> GetAssetByIdAsync(string id)
        {
            var asset = await _repository.GetByIdAsync(id);
            return _mapper.Map<AssetDTO>(asset);
        }

        public async Task<AssetDTO> CreateAssetAsync(CreateAssetDTO createDto)
        {
            var asset = _mapper.Map<GamingAsset>(createDto);

            asset.Status = Status.Available;

            await _repository.CreateAsync(asset);

            return _mapper.Map<AssetDTO>(asset);
        }

        public async Task<bool> UpdateAssetAsync(string id, UpdateAssetDTO updateDto)
        {
            var existingAsset = await _repository.GetByIdAsync(id);
            if (existingAsset == null)
            {
                return false;
            }

            var assetToUpdate = _mapper.Map<GamingAsset>(updateDto);
            assetToUpdate.Id = id;

            return await _repository.UpdateAsync(id, assetToUpdate);
        }

        public async Task<bool> DeleteAssetAsync(string id)
        {
            var existingAsset = await _repository.GetByIdAsync(id);
            if (existingAsset == null) return false;

            if (existingAsset.Status == Status.Rented) return false;

            return await _repository.DeleteAsync(id);
        }
        public async Task<string> GetTotalValueByStatusAsync(Status status)
        {
            var assets = await _repository.GetAllAsync();

            var totalValue = assets
                .Where(a => a.Status == status).Max(a => a.Name);

            return totalValue;
        }

        public async Task<AssetDTO> GetMostExpensiveAssetAsync()
        {
            var assets = await _repository.GetAllAsync();

            if (assets == null || !assets.Any())
            {
                throw new InvalidOperationException("No assets found in the database.");
            }

            var expensiveAsset = assets.OrderByDescending(a => a.Name).FirstOrDefault();

            return _mapper.Map<AssetDTO>(expensiveAsset);
        }
    }
}
