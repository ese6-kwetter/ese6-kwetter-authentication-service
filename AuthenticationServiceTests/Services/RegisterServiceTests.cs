using System;
using System.Threading.Tasks;
using AuthenticationService.Entities;
using AuthenticationService.Exceptions;
using AuthenticationService.Helpers;
using AuthenticationService.Repositories;
using AuthenticationService.Services;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace AuthenticationServiceTests.Services
{
    [TestFixture]
    public class RegisterServiceTests
    {
        private Mock<IUserRepository> _repository;
        private Mock<IHashGenerator> _hashGenerator;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IUserRepository>();
            _hashGenerator = new Mock<IHashGenerator>();
        }

        [Test]
        public async Task RegisterPassword_ReturnUserWithoutPassword()
        {
            // Arrange
            const string username = "test";
            const string email = "test@test.com";
            const string password = "test";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};
            
            _hashGenerator.Setup(h => h.Salt()).Returns(salt);
            _hashGenerator.Setup(h => h.Hash(password, salt))
                .Returns(hashedPassword);
            
            var user = new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword,
                Salt = salt,
            };
            
            _repository.Setup(r => r.Create(user))
                .ReturnsAsync(user);

            var service = new RegisterService(_repository.Object, _hashGenerator.Object);

            // Act
            var result = await service.RegisterPassword(username, email, password);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(hashedPassword, result.Password);
        }

        [Test]
        public async Task RegisterPassword_ExistingUsername_ThrowException()
        {
            // Arrange
            const string username = "test";
            const string email = "test@test.com";
            const string password = "test";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};
            
            _hashGenerator.Setup(h => h.Salt()).Returns(salt);
            _hashGenerator.Setup(h => h.Hash(password, salt))
                .Returns(hashedPassword);
            
            var user = new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword,
                Salt = salt,
            };
            _repository.Setup(r => r.ReadByUsername(username)).ReturnsAsync(user);

            var service = new RegisterService(_repository.Object, _hashGenerator.Object);
            
            // Assert
            Assert.ThrowsAsync<UsernameAlreadyExistsException>(() => service.RegisterPassword(username, email, password));
        }

        [Test]
        public async Task RegisterPassword_ExistingEmail_ThrowException()
        {
            // Arrange
            const string username = "test";
            const string email = "test@test.com";
            const string password = "test";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};
            
            _hashGenerator.Setup(h => h.Salt()).Returns(salt);
            _hashGenerator.Setup(h => h.Hash(password, salt))
                .Returns(hashedPassword);
            
            var user = new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword,
                Salt = salt,
            };
            _repository.Setup(r => r.ReadByEmail(email)).ReturnsAsync(user);

            var service = new RegisterService(_repository.Object, _hashGenerator.Object);

            // Assert
            Assert.ThrowsAsync<EmailAlreadyExistsException>(() => service.RegisterPassword(username, email, password));
        }
    }
}
