using LW_4_3_5_Daryev_PI231.DTOs;

namespace LW_4_3_5_Daryev_PI231.Services
{
    public interface IGamingAssetService
    {
        Task<IEnumerable<AssetDTO>> GetAllAssetsAsync();
        Task<AssetDTO> GetAssetByIdAsync(string id);
        Task<AssetDTO> CreateAssetAsync(CreateAssetDTO createDto);
        Task<bool> UpdateAssetAsync(string id, UpdateAssetDTO updateDto);
        Task<bool> DeleteAssetAsync(string id);
    }
}