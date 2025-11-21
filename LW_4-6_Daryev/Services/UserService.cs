using LW_4_3_5_Daryev_PI231.Repositories;

namespace LW_4_3_5_Daryev_PI231.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateAsync(Models.User user) => await _userRepository.CreateUserAsync(user);
        public async Task<List<Models.User>> GetAsync() => await _userRepository.GetAllUsersAsync();
        public async Task<Models.User?> GetByEmailAsync(string email) => await _userRepository.GetUserByEmailAsync(email);
        public async Task<Models.User> GetAsync(string id) => await _userRepository.GetUserByIdAsync(id);
        public async Task UpdateAsync(Models.User user) => await _userRepository.UpdateUserAsync(user);
        public async Task DeleteAsync(string id) => await _userRepository.DeleteUserAsync(id);
    }
}
