using System.Collections.Generic;
using NUnit.Framework;
using UserMicroservice.Entities;
using UserMicroservice.Helpers;

namespace UserMicroserviceTests.Helpers
{
    [TestFixture]
    public class ExtensionMethodsTests
    {
        [Test]
        public void WithoutPassword_UserWithPassword_ReturnsUserWithoutPassword()
        {
            // Arrange
            const string username = "username";
            const string email = "test@test.com";
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};

            var user = new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword
            };

            // Act
            var result = user.WithoutSensitiveData();

            // Assert
            Assert.AreEqual(username, result.Username);
            Assert.AreEqual(email, result.Email);
            Assert.IsNull(user.Password);
        }

        [Test]
        public void WithoutPasswords_UsersWithPasswords_ReturnsUsersWithoutPasswords()
        {
            // Arrange
            const string username = "username";
            const string email = "test@test.com";
            var hashedPassword = new byte[] {0x20, 0x20, 0x20, 0x20};

            var user = new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword
            };

            var users = new List<User>();

            for (var i = 0; i < 5; i++)
                users.Add(user);

            // Act
            var result = users.WithoutSensitiveData();

            // Assert
            foreach (var r in result)
            {
                Assert.AreEqual(username, r.Username);
                Assert.AreEqual(email, r.Email);
                Assert.IsNull(r.Password);
            }
        }
    }
}
