using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserService.Entities;
using UserService.Exceptions;
using UserService.Helpers;
using UserService.Repositories;
using UserService.Services;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace UserServiceTests.Services
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
        [Ignore("The mocking of UserRepository.CreateAsync() returns null instead of a User")]
        public async Task RegisterPassword_UserWithPassword_ReturnsUserWithoutPassword()
        {
            // Arrange
            const string username = "test";
            const string email = "test@test.com";
            const string password = "test";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};
            
            _hashGenerator.Setup(h => h.Salt()).Returns(salt);
            _hashGenerator.Setup(h => h.Hash(password, salt)).Returns(hashedPassword);
            
            var user = new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword,
                Salt = salt
            };
            
            _repository.Setup(r => r.CreateAsync(user)).ReturnsAsync(user);

            var service = new RegisterService(_repository.Object, _hashGenerator.Object);

            // Act
            var result = await service.RegisterPasswordAsync(username, email, password);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(hashedPassword, result.Password);
        }

        [Test]
        public async Task RegisterPassword_ExistingUsername_ThrowsUsernameAlreadyExistsException()
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
                Salt = salt
            };
            
            _repository.Setup(r => r.ReadByUsernameAsync(username)).ReturnsAsync(user);

            var service = new RegisterService(_repository.Object, _hashGenerator.Object);
            
            // Act and assert
            Assert.ThrowsAsync<UsernameAlreadyExistsException>(() => service.RegisterPasswordAsync(username, email, password));
        }

        [Test]
        public async Task RegisterPassword_ExistingEmail_ThrowsEmailAlreadyExistsException()
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
            
            _repository.Setup(r => r.ReadByEmailAsync(email)).ReturnsAsync(user);

            var service = new RegisterService(_repository.Object, _hashGenerator.Object);

            // Act and assert
            Assert.ThrowsAsync<EmailAlreadyExistsException>(() => service.RegisterPasswordAsync(username, email, password));
        }
    }
}
