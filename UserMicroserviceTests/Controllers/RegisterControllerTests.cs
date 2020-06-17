using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using UserMicroservice.Controllers;
using UserMicroservice.Entities;
using UserMicroservice.Exceptions;
using UserMicroservice.Models;
using UserMicroservice.Services;

namespace UserMicroserviceTests.Controllers
{
    [TestFixture]
    public class RegisterControllerTests
    {
        [SetUp]
        public void SetUp()
        {
            _service = new Mock<IRegisterService>();
        }

        private Mock<IRegisterService> _service;

        [Test]
        public async Task RegisterPassword_UserWithExistingEmail_ReturnsBadRequestObjectResult()
        {
            // Arrange
            const string username = "username";
            const string email = "test@test.com";
            const string password = "password";

            var registerModel = new RegisterPasswordModel
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
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task RegisterPassword_UserWithExistingUsername_ReturnsBadRequestObjectResult()
        {
            // Arrange
            const string username = "username";
            const string email = "test@test.com";
            const string password = "password";

            var registerModel = new RegisterPasswordModel
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
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task RegisterPassword_UserWithPassword_ReturnsUser()
        {
            // Arrange
            var id = Guid.NewGuid();
            const string username = "username";
            const string email = "test@test.com";
            const string password = "password";

            var registerModel = new RegisterPasswordModel
            {
                Username = username,
                Email = email,
                Password = password
            };

            var user = new User
            {
                Id = id,
                Username = username,
                Email = email
            };

            _service.Setup(s => s.RegisterPasswordAsync(username, email, password))
                .ReturnsAsync(user);

            var controller = new RegisterController(_service.Object);

            // Act
            var result = await controller.RegisterPasswordAsync(registerModel) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(user, result.Value);
        }
    }
}
