using MongoDB.Driver;

namespace LW_4_3_5_Daryev_PI231.Repositories
{
    public class UserRepository : IUserRepository
    {
        IMongoCollection<Models.User> _usersCollection;
        public UserRepository(IMongoDatabase database)
        {
            _usersCollection = database.GetCollection<Models.User>("users");
        }
        async Task IUserRepository.CreateUserAsync(Models.User user) => await _usersCollection.InsertOneAsync(user);
        async Task<List<Models.User>> IUserRepository.GetAllUsersAsync() => await _usersCollection.Find(_ => true).ToListAsync();
        async Task<Models.User?> IUserRepository.GetUserByEmailAsync(string email) => await _usersCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
        async Task<Models.User?> IUserRepository.GetUserByIdAsync(string id) => await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        async Task IUserRepository.UpdateUserAsync(Models.User user) => await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
        async Task IUserRepository.DeleteUserAsync(string id) => await _usersCollection.DeleteOneAsync(u => u.Id == id);


    }
}
