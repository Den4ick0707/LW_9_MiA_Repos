using LW_4_3_5_Daryev_PI231.Models;

namespace LW_4_3_5_Daryev_PI231.Services
{
    public interface IUserService
    {
        Task CreateAsync(User todoItem);
        Task<List<User>> GetAsync();
        Task<User?> GetByEmailAsync(string email);
        Task<User> GetAsync(string id);
        Task UpdateAsync(User todoItem);
        Task DeleteAsync(string id);
    }
}
