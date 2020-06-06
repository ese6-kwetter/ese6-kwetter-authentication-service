using System.Threading.Tasks;
using MongoDB.Driver;
using UserMicroservice.Entities;
using UserMicroservice.Settings;

namespace UserMicroservice.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDatabaseSettings databaseSettings) : base(databaseSettings)
        {
        }

        public async Task<User> ReadByUsernameAsync(string username)
        {
            return await Collection.Find(user => user.Username == username).FirstOrDefaultAsync();
        }

        public async Task<User> ReadByEmailAsync(string email)
        {
            return await Collection.Find(user => user.Email == email).FirstOrDefaultAsync();
        }
    }
}
