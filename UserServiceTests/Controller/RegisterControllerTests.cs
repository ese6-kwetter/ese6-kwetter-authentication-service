using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using UserService.Controllers;
using UserService.Entities;
using UserService.Exceptions;
using UserService.Models;
using UserService.Repositories;
using UserService.Services;

namespace UserServiceTests.Controller
{
    [TestFixture]
    public class RegisterControllerTests
    {
        private Mock<IRegisterService> _service;

        [SetUp]
        public void SetUp()
        {
            _service = new Mock<IRegisterService>();
        }

        [Test]
        public async Task RegisterPassword_UserWithPassword_ReturnUserWithoutPassword()
        {
            // Arrange
            const string username = "test";
            const string email = "test@test.com";
            const string password = "test";
            var salt = new byte[] {0x20, 0x20, 0x20, 0x20};
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};
            
            var registerModel = new RegisterModel()
            {
                Username = username,
                Email = email,
                Password = password
            };
            
            var user = new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword,
                Salt = salt
            };
            
            _service.Setup(s => s.RegisterPasswordAsync(username, email, password))
                .ReturnsAsync(user);
            
            var controller = new RegisterController(_service.Object);
            
            // Act
            var result = await controller.RegisterPasswordAsync(registerModel) as ObjectResult;
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user, result.Value);
        }

        [Test]
        public async Task RegisterPassword_UserWithExistingUsername_ReturnBadRequest()
        {
            // Arrange
            const string username = "test";
            const string email = "test@test.com";
            const string password = "test";
            
            var registerModel = new RegisterModel()
            {
                Username = username,
                Email = email,
                Password = password
            };
            
            _service.Setup(s => s.RegisterPasswordAsync(username, email, password))
                .Throws<UsernameAlreadyExistsException>();
            
            var controller = new RegisterController(_service.Object);
            
            // Act
            var result = await controller.RegisterPasswordAsync(registerModel) as ObjectResult;
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new UsernameAlreadyExistsException().Message, result.Value);
        }

        [Test]
        public async Task RegisterPassword_UserWithExistingEmail_ReturnBadRequest()
        {
            // Arrange
            const string username = "test";
            const string email = "test@test.com";
            const string password = "test";
            
            var registerModel = new RegisterModel()
            {
                Username = username,
                Email = email,
                Password = password
            };
            
            _service.Setup(s => s.RegisterPasswordAsync(username, email, password))
                .Throws<EmailAlreadyExistsException>();
            
            var controller = new RegisterController(_service.Object);
            
            // Act
            var result = await controller.RegisterPasswordAsync(registerModel) as ObjectResult;
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new EmailAlreadyExistsException().Message, result.Value);
        }
    }
}
