using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using UserService.Entities;
using UserService.Settings;

namespace UserService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IUserDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UserCollectionName);
        }
        
        public async Task<User> CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
            
            return user;
        }

        public async Task<List<User>> ReadAsync() =>
            await _users.Find(user => true).ToListAsync();

        public async Task<User> ReadByIdAsync(Guid id) =>
            await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
        
        public async Task<User> ReadByUsernameAsync(string username) =>
            await _users.Find(user => user.Username == username).FirstOrDefaultAsync();
        
        public async Task<User> ReadByEmailAsync(string email) =>
            await _users.Find(user => user.Email == email).FirstOrDefaultAsync();

        public async Task UpdateAsync(Guid id, User userIn) =>
            await _users.ReplaceOneAsync(user => user.Id == id, userIn);

        public async Task DeleteAsync(User userIn) =>
            await _users.DeleteOneAsync(user => user.Id == userIn.Id);

        public async Task DeleteAsync(Guid id) =>
            await _users.DeleteOneAsync(user => user.Id == id);
    }
}
