using LW_4_3_5_Daryev_PI231.Models;

namespace LW_4_3_5_Daryev_PI231.Repositories
{
    public interface IGamingAssetRepository
    {
        Task<IEnumerable<GamingAsset>> GetAllAsync();
        Task<GamingAsset> GetByIdAsync(string id);
        Task CreateAsync(GamingAsset asset);
        Task<bool> UpdateAsync(string id, GamingAsset asset);
        Task<bool> DeleteAsync(string id);
    }
}