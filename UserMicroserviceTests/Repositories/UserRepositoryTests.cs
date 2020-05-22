using System.Threading.Tasks;
using Mongo2Go;
using NUnit.Framework;
using UserMicroservice.Entities;
using UserMicroservice.Repositories;
using UserMicroservice.Settings;

namespace UserMicroserviceTests.Repositories
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
                Username = "username1",
                Email = "test1@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };
            
            var user2 = new User
            {
                Username = "username2",
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
                Username = "username1",
                Email = "test1@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };
            
            var user2 = new User
            {
                Username = "username2",
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
        public async Task ReadByIdAsync_UsersInDatabase_ReturnsUser()
        {
            // Arrange
            var user1 = new User
            {
                Username = "username1",
                Email = "test1@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };
            
            var user2 = new User
            {
                Username = "username2",
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

        [Test]
        public async Task ReadByIdAsync_NoUsersInDatabase_ReturnsNull()
        {
            // Arrange
            var user1 = new User
            {
                Username = "username1",
                Email = "test1@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };
            
            // Act
            var result = await _repository.ReadByIdAsync(user1.Id);
            
            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task ReadByUsernameAsync_UsersInDatabase_ReturnsUser()
        {
            // Arrange
            var user1 = new User
            {
                Username = "username1",
                Email = "test1@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };
            
            var user2 = new User
            {
                Username = "username2",
                Email = "test2@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };

            await _repository.CreateAsync(user1);
            await _repository.CreateAsync(user2);
            
            // Act
            var result = await _repository.ReadByUsernameAsync(user1.Username);
            
            // Assert
            Assert.AreEqual(user1.Id, result.Id);
            Assert.AreNotEqual(user2.Id, result.Id);
        }

        [Test]
        public async Task ReadByUsernameAsync_NoUsersInDatabase_ReturnsNull()
        {
            // Arrange
            var user1 = new User
            {
                Username = "username1",
                Email = "test1@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };
            
            // Act
            var result = await _repository.ReadByUsernameAsync(user1.Username);
            
            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task ReadByEmailAsync_UsersInDatabase_ReturnsUser()
        {
            // Arrange
            var user1 = new User
            {
                Username = "username1",
                Email = "test1@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };
            
            var user2 = new User
            {
                Username = "username2",
                Email = "test2@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };

            await _repository.CreateAsync(user1);
            await _repository.CreateAsync(user2);
            
            // Act
            var result = await _repository.ReadByEmailAsync(user1.Email);
            
            // Assert
            Assert.AreEqual(user1.Id, result.Id);
            Assert.AreNotEqual(user2.Id, result.Id);
        }

        [Test]
        public async Task ReadByEmailAsync_NoUsersInDatabase_ReturnsNull()
        {
            // Arrange
            var user1 = new User
            {
                Username = "username",
                Email = "test@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };
            
            // Act
            var result = await _repository.ReadByEmailAsync(user1.Email);
            
            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateAsync_UserInDatabase_ReturnsUser()
        {
            // Arrange
            const string username = "username1";
            const string email = "test1@test.com";
            var user = new User
            {
                Username = "test",
                Email = "test@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };

            user = await _repository.CreateAsync(user);
            
            // Act
            user.Username = username;
            user.Email = email;
            
            var result = await _repository.UpdateAsync(user.Id, user);
            
            // Assert
            Assert.AreEqual(username, result.Username);
            Assert.AreEqual(email, result.Email);
        }

        [Test]
        public async Task DeleteByIdAsync_UserInDatabase_ReturnsNull()
        {
            // Arrange
            var user = new User
            {
                Username = "username",
                Email = "test@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };

            user = await _repository.CreateAsync(user);
            
            // Act
            await _repository.DeleteByIdAsync(user.Id);
            var result = await _repository.ReadByIdAsync(user.Id);
            
            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task DeleteByUserAsync_UserInDatabase_ReturnsNull()
        {
            // Arrange
            var user = new User
            {
                Username = "username",
                Email = "test@test.com",
                Password = new byte[] { 0x20, 0x20, 0x20, 0x20 },
                Salt = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };

            user = await _repository.CreateAsync(user);
            
            // Act
            await _repository.DeleteByUserAsync(user);
            var result = await _repository.ReadByIdAsync(user.Id);
            
            // Assert
            Assert.IsNull(result);
        }
    }
}
