using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthenticationService.DataStoreSettings;
using AuthenticationService.Entities;
using MongoDB.Driver;

namespace AuthenticationService.Repositories
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
        
        public async Task<User> Create(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<List<User>> Read() =>
            await _users.Find(user => true).ToListAsync();

        public async Task<User> Read(string username) =>
            await _users.Find(user => user.Username == username).FirstOrDefaultAsync();

        public async Task<User> Read(Guid id) =>
            await _users.Find(user => user.Id == id).FirstOrDefaultAsync();

        public async Task Update(Guid id, User userIn) =>
            await _users.ReplaceOneAsync(user => user.Id == id, userIn);

        public async Task Delete(User userIn) =>
            await _users.DeleteOneAsync(user => user.Id == userIn.Id);

        public async Task Delete(Guid id) =>
            await _users.DeleteOneAsync(user => user.Id == id);
    }
}
