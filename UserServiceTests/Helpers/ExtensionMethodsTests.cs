using System.Collections.Generic;
using NUnit.Framework;
using UserService.Entities;
using UserService.Helpers;

namespace UserServiceTests.Helpers
{
    [TestFixture]
    public class ExtensionMethodsTests
    {
        [Test]
        public void WithoutPassword_UserWithPassword_ReturnsUserWithoutPassword()
        {
            // Arrange
            var user = new User
            {
                Username = "test",
                Email = "test@test.com",
                Password = new byte[] {0x20, 0x20, 0x20, 0x20}
            };

            // Act
            var result = user.WithoutSensitiveData();

            // Assert
            Assert.IsNotNull(user.Username);
            Assert.IsNotNull(user.Email);
            Assert.IsNull(user.Password);
        }

        [Test]
        public void WithoutPasswords_UsersWithPasswords_ReturnsUsersWithoutPasswords()
        {
            // Arrange
            var users = new List<User>();
            for (var i = 0; i < 5; i++)
                users.Add(new User
                {
                    Username = "test",
                    Email = "test@test.com",
                    Password = new byte[] {0x20, 0x20, 0x20, 0x20}
                });

            // Act
            var result = users.WithoutSensitiveData();

            // Assert
            foreach (var user in result)
            {
                Assert.IsNotNull(user.Username);
                Assert.IsNotNull(user.Email);
                Assert.IsNull(user.Password);
            }
        }
    }
}
