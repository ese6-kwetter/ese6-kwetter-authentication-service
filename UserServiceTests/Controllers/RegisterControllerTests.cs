﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using UserService.Controllers;
using UserService.Entities;
using UserService.Exceptions;
using UserService.Helpers;
using UserService.Models;
using UserService.Repositories;
using UserService.Services;

namespace UserServiceTests.Controllers
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
        public async Task RegisterPassword_UserWithPassword_ReturnsUser()
        {
            // Arrange
            var id = Guid.NewGuid();
            const string username = "test";
            const string email = "test@test.com";
            const string password = "test";
            
            var registerModel = new RegisterModel()
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
            Assert.AreEqual(user, result.Value);
        }

        [Test]
        public async Task RegisterPassword_UserWithExistingUsername_ReturnsBadRequest()
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
        public async Task RegisterPassword_UserWithExistingEmail_ReturnsBadRequest()
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