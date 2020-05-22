using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using UserMicroservice.Entities;
using UserMicroservice.Exceptions;
using UserMicroservice.Helpers;
using UserMicroservice.Repositories;
using UserMicroservice.Services;

namespace UserMicroserviceTests.Services
{
    public class LoginServiceTests
    {
        private Mock<IUserRepository> _repository;
        private Mock<IHashGenerator> _hashGenerator;
        private Mock<ITokenGenerator> _tokenGenerator;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IUserRepository>();
            _hashGenerator = new Mock<IHashGenerator>();
            _tokenGenerator = new Mock<ITokenGenerator>();
        }

        [Test]
        public async Task LoginPasswordAsync_UserWithPassword_ReturnsUserWithJwtToken()
        {
            // Arrange
            var id = new Guid();
            const string username = "username";
            const string email = "test@test.com";
            const string password = "password";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};
            const string jwt = "jwt";

            _hashGenerator.Setup(h => h.Verify(password, salt, hashedPassword)).Returns(true);
            _tokenGenerator.Setup(t => t.GenerateJwt(id)).Returns(jwt);

            var user = new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword,
                Salt = salt
            };

            _repository.Setup(r => r.ReadByEmailAsync(email)).ReturnsAsync(user);

            var service = new LoginService(_repository.Object, _hashGenerator.Object, _tokenGenerator.Object);
            
            // Act
            var result = await service.LoginPasswordAsync(email, password);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(jwt, result.Jwt);
        }

        [Test]
        public async Task LoginPasswordAsync_UserWithIncorrectEmail_ThrowsEmailNotFoundException()
        {
            // Arrange
            const string email = "test@test.com";
            const string password = "password";

            _repository.Setup(r => r.ReadByEmailAsync(email));

            var service = new LoginService(_repository.Object, _hashGenerator.Object, _tokenGenerator.Object);

            // Act and assert
            Assert.ThrowsAsync<EmailNotFoundException>(() => service.LoginPasswordAsync(email, password));
        }

        [Test]
        public async Task LoginPasswordAsync_UserWithIncorrectPassword_ThrowsIncorrectPasswordException()
        {
            
            // Arrange
            var id = new Guid();
            const string username = "username";
            const string email = "test@test.com";
            const string password = "password";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};

            _hashGenerator.Setup(h => h.Verify(password, salt, hashedPassword)).Returns(false);

            var user = new User
            {
                Id = id,
                Username = username,
                Email = email,
                Password = hashedPassword,
                Salt = salt
            };

            _repository.Setup(r => r.ReadByEmailAsync(email)).ReturnsAsync(user);

            var service = new LoginService(_repository.Object, _hashGenerator.Object, _tokenGenerator.Object);
            
            // Act and assert
            Assert.ThrowsAsync<InvalidPasswordException>(() => service.LoginPasswordAsync(email, password));
        }
    }
}
