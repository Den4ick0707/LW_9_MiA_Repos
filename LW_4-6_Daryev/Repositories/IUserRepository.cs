namespace LW_4_3_5_Daryev_PI231.Repositories
{
    public interface IUserRepository
    {

        Task CreateUserAsync(Models.User user);
        Task<List<Models.User>> GetAllUsersAsync();
        Task<Models.User?> GetUserByIdAsync(string id);
        Task<Models.User?> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(Models.User user);
        Task DeleteUserAsync(string id);
    }
}
