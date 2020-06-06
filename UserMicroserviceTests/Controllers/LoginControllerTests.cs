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
    public class LoginControllerTests
    {
        [SetUp]
        public void SetUp()
        {
            _service = new Mock<ILoginService>();
        }

        private Mock<ILoginService> _service;

        [Test]
        public async Task LoginPassword_EmailWithPassword_ReturnsUser()
        {
            // Arrange
            var id = Guid.NewGuid();
            const string username = "username";
            const string email = "test@test.com";
            const string password = "password";
            const string jwt = "jwt";

            var loginModel = new LoginModel
            {
                Email = email,
                Password = password
            };

            var user = new User
            {
                Id = id,
                Username = username,
                Email = email,
                Jwt = jwt
            };

            _service.Setup(s => s.LoginPasswordAsync(email, password))
                .ReturnsAsync(user);

            var controller = new LoginController(_service.Object);

            // Act
            var result = await controller.LoginPasswordAsync(loginModel) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(user, result.Value);
        }

        [Test]
        public async Task LoginPassword_EmailWithWrongPassword_ReturnsBadRequestObjectResult()
        {
            // Arrange
            const string email = "test@test.com";
            const string password = "password";

            var loginModel = new LoginModel
            {
                Email = email,
                Password = password
            };

            _service.Setup(s => s.LoginPasswordAsync(email, password))
                .Throws<InvalidPasswordException>();

            var controller = new LoginController(_service.Object);

            // Act
            var result = await controller.LoginPasswordAsync(loginModel) as ObjectResult;

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task LoginPassword_WrongEmailWithPassword_ReturnsNotFoundObjectResult()
        {
            // Arrange
            const string email = "test@test.com";
            const string password = "password";

            var loginModel = new LoginModel
            {
                Email = email,
                Password = password
            };

            _service.Setup(s => s.LoginPasswordAsync(email, password))
                .Throws<EmailNotFoundException>();

            var controller = new LoginController(_service.Object);

            // Act
            var result = await controller.LoginPasswordAsync(loginModel) as ObjectResult;

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
    }
}
