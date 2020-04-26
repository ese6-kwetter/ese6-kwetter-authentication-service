using System.Threading.Tasks;
using Mongo2Go;
using NUnit.Framework;
using UserService.Entities;
using UserService.Repositories;
using UserService.Settings;

namespace UserServiceTests.Repositories
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private IUserRepository _repository;
        private MongoDbRunner _mongoDbRunner;

        [SetUp]
        public void SetUp()
        {
            _mongoDbRunner = MongoDbRunner.Start();

            var settings = new UserDatabaseSettings
            {
                ConnectionString = _mongoDbRunner.ConnectionString,
                DatabaseName = "RepositoryTests",
                UserCollectionName = "UserCollection"
            };

            _repository = new UserRepository(settings);
        }

        [TearDown]
        public void TearDown()
        {
            _mongoDbRunner.Dispose();
        }

        [Test]
        public async Task CreateAsync_CreateTwoUsers_ReturnsCreatedUsers()
        {
            // Arrange
            var user1 = new User
            {
                Username = "test1",
                Email = "test1@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };
            
            var user2 = new User
            {
                Username = "test2",
                Email = "test2@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };

            //Act
            var result1 = await _repository.CreateAsync(user1);
            var result2 = await _repository.CreateAsync(user2);
            
            // Assert
            Assert.AreEqual(user1.Id, result1.Id);
            Assert.AreEqual(user2.Id, result2.Id);
            Assert.AreNotEqual(user1.Id, user2.Id);
        }

        [Test]
        public async Task ReadAsync_CreateUsersInDatabase_ReturnsAllUsers()
        {
            // Arrange
            var user1 = new User
            {
                Username = "test1",
                Email = "test1@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };
            
            var user2 = new User
            {
                Username = "test2",
                Email = "test2@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };
            
            await _repository.CreateAsync(user1);
            await _repository.CreateAsync(user2);
            
            // Act
            var result = await _repository.ReadAsync();
            
            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(user1.Id, result[0].Id);
            Assert.AreEqual(user2.Id, result[1].Id);
        }

        [Test]
        public async Task ReadAsync_NoUsersInDatabase_ReturnsEmptyList()
        {
            // Act
            var result = await _repository.ReadAsync();
            
            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task ReadByIdAsync_CreateUserInDatabase_ReturnsUser()
        {
            // Arrange
            var user1 = new User
            {
                Username = "test1",
                Email = "test1@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };
            
            var user2 = new User
            {
                Username = "test2",
                Email = "test2@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };

            await _repository.CreateAsync(user1);
            await _repository.CreateAsync(user2);
            
            // Act
            var result = await _repository.ReadByIdAsync(user1.Id);
            
            // Assert
            Assert.AreEqual(user1.Id, result.Id);
            Assert.AreNotEqual(user2.Id, result.Id);
        }
    }
}
