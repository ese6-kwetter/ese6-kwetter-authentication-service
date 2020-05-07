using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using UserMicroservice.Entities;
using UserMicroservice.Settings;

namespace UserMicroservice.Repositories
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

        public async Task<User> UpdateAsync(Guid id, User userIn)
        {
            await _users.ReplaceOneAsync(user => user.Id == id, userIn);
            
            return await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task DeleteByIdAsync(Guid id) =>
            await _users.DeleteOneAsync(user => user.Id == id);

        public async Task DeleteByUserAsync(User userIn) =>
            await _users.DeleteOneAsync(user => user.Id == userIn.Id);
    }
}
