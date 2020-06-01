using System;
using System.Threading;
using System.Threading.Tasks;
using MessageBroker;
using Microsoft.AspNetCore.Mvc;
using UserMicroservice.Entities;
using UserMicroservice.Exceptions;
using UserMicroservice.Helpers;
using UserMicroservice.Repositories;
using UserMicroservice.Services;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace UserMicroserviceTests.Services
{
    [TestFixture]
    public class RegisterServiceTests
    {
        private Mock<IUserRepository> _repository;
        private Mock<IHashGenerator> _hashGenerator;
        private Mock<IRegexValidator> _regexValidator;
        private Mock<IMessageQueuePublisher> _messageQueuePublisher;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IUserRepository>();
            _hashGenerator = new Mock<IHashGenerator>();
            _regexValidator = new Mock<IRegexValidator>();
            _messageQueuePublisher = new Mock<IMessageQueuePublisher>();
        }

        [Test]
        [Ignore("The mocking of UserRepository.CreateAsync() returns null instead of a User")]
        public async Task RegisterPasswordAsync_UserWithPassword_ReturnsUserWithoutSensitiveData()
        {
            // Arrange
            const string username = "username";
            const string email = "test@test.com";
            const string password = "password";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};
            
            _regexValidator.Setup(r => r.IsValidEmail(email)).Returns(true);
            _regexValidator.Setup(r => r.IsValidPassword(password)).Returns(true);

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

            var service = new RegisterService(_repository.Object, _regexValidator.Object, _hashGenerator.Object, _messageQueuePublisher.Object);

            // Act
            var result = await service.RegisterPasswordAsync(username, email, password);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(hashedPassword, result.Password);
        }

        [Test]
        public async Task RegisterPasswordAsync_ExistingUsername_ThrowsUsernameAlreadyExistsException()
        {
            // Arrange
            const string username = "username";
            const string email = "test@test.com";
            const string password = "password";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};
            
            _regexValidator.Setup(r => r.IsValidEmail(email)).Returns(true);
            _regexValidator.Setup(r => r.IsValidPassword(password)).Returns(true);

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

            var service = new RegisterService(_repository.Object, _regexValidator.Object, _hashGenerator.Object, _messageQueuePublisher.Object);

            // Act and assert
            Assert.ThrowsAsync<UsernameAlreadyExistsException>(
                () => service.RegisterPasswordAsync(username, email, password)
            );
        }

        [Test]
        public async Task RegisterPasswordAsync_ExistingEmail_ThrowsEmailAlreadyExistsException()
        {
            // Arrange
            const string username = "username";
            const string email = "test@test.com";
            const string password = "password";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};

            _regexValidator.Setup(r => r.IsValidEmail(email)).Returns(true);
            _regexValidator.Setup(r => r.IsValidPassword(password)).Returns(true);

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

            var service = new RegisterService(_repository.Object, _regexValidator.Object, _hashGenerator.Object, _messageQueuePublisher.Object);

            // Act and assert
            Assert.ThrowsAsync<EmailAlreadyExistsException>(
                () => service.RegisterPasswordAsync(username, email, password)
            );
        }
    }
}
