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
        private readonly IMongoCollection<User> _userCollection;

        public UserRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _userCollection = database.GetCollection<User>(databaseSettings.CollectionName);
        }
        
        public async Task<User> CreateAsync(User user)
        {
            await _userCollection.InsertOneAsync(user);
            
            return user;
        }

        public async Task<List<User>> ReadAsync() =>
            await _userCollection.Find(user => true).ToListAsync();

        public async Task<User> ReadByIdAsync(Guid id) =>
            await _userCollection.Find(user => user.Id == id).FirstOrDefaultAsync();
        
        public async Task<User> ReadByUsernameAsync(string username) =>
            await _userCollection.Find(user => user.Username == username).FirstOrDefaultAsync();
        
        public async Task<User> ReadByEmailAsync(string email) =>
            await _userCollection.Find(user => user.Email == email).FirstOrDefaultAsync();

        public async Task<User> UpdateAsync(Guid id, User userIn)
        {
            await _userCollection.ReplaceOneAsync(user => user.Id == id, userIn);
            
            return await _userCollection.Find(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task DeleteByIdAsync(Guid id) =>
            await _userCollection.DeleteOneAsync(user => user.Id == id);

        public async Task DeleteByUserAsync(User userIn) =>
            await _userCollection.DeleteOneAsync(user => user.Id == userIn.Id);
    }
}
