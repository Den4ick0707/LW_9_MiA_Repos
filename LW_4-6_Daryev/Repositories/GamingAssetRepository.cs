using LW_4_3_5_Daryev_PI231.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LW_4_3_5_Daryev_PI231.Repositories
{
    public class GamingAssetRepository : IGamingAssetRepository
    {
        private readonly IMongoCollection<GamingAsset> _assetsCollection;

        public GamingAssetRepository(IMongoDatabase database)
        {
            _assetsCollection = database.GetCollection<GamingAsset>("GamingAssets");
        }

        public async Task<IEnumerable<GamingAsset>> GetAllAsync() =>
            await _assetsCollection.Find(_ => true).ToListAsync();

        public async Task<GamingAsset> GetByIdAsync(string id) =>
            await _assetsCollection.Find(a => a.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(GamingAsset asset) =>
            await _assetsCollection.InsertOneAsync(asset);

        public async Task<bool> UpdateAsync(string id, GamingAsset asset)
        {
            var result = await _assetsCollection.ReplaceOneAsync(a => a.Id == id, asset);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _assetsCollection.DeleteOneAsync(a => a.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}