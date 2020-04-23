using System;
using UserService.Helpers;
using NUnit.Framework;

namespace UserServiceTests.Helpers
{
    [TestFixture]
    public class HashGeneratorTests
    {
        private IHashGenerator _hashGenerator;

        [SetUp]
        public void Setup()
        {
            _hashGenerator = new HashGenerator();
        }

        [Test]
        public void Hash_AreNotEqual_ReturnTrue()
        {
            // Arrange
            const string plainText = "test";
            var salt = _hashGenerator.Salt();

            // Act
            var result = _hashGenerator.Hash(plainText, salt);

            // Assert
            Assert.AreNotEqual(plainText, result);
        }
        
        [Test]
        public void Verify_ReturnTrue()
        {
            // Arrange
            const string plainText = "test";
            var salt = _hashGenerator.Salt();
            var hash = _hashGenerator.Hash(plainText, salt);

            // Act
            var result = _hashGenerator.Verify(plainText, salt, hash);

            // Assert
            Assert.IsTrue(result);
        }
        
        [Test]
        public void Verify_IncorrectString_ReturnFalse()
        {
            // Arrange
            const string plainText = "test";
            const string plainText2 = "test2";
            var salt = _hashGenerator.Salt();
            var hash = _hashGenerator.Hash(plainText, salt);

            // Act
            var result = _hashGenerator.Verify(plainText2, salt, hash);

            // Assert
            Assert.IsFalse(result);
        }
        
        [Test]
        public void Verify_IncorrectSalt_ReturnFalse()
        {
            // Arrange
            const string plainText = "test";
            var salt = _hashGenerator.Salt();
            var incorrectSalt = _hashGenerator.Salt();
            var hash = _hashGenerator.Hash(plainText, salt);

            // Act
            var result = _hashGenerator.Verify(plainText, incorrectSalt, hash);

            // Assert
            Assert.IsFalse(result);
        }
        
        [Test]
        public void Verify_IncorrectStringAndSalt_ReturnFalse()
        {
            // Arrange
            const string plainText = "test";
            const string plainText2 = "test2";
            var salt = _hashGenerator.Salt();
            var incorrectSalt = _hashGenerator.Salt();
            var hash = _hashGenerator.Hash(plainText, salt);

            // Act
            var result = _hashGenerator.Verify(plainText2, incorrectSalt, hash);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Salt_AreNotEqual_ReturnTrue()
        {
            // Arrange and act
            var salt = _hashGenerator.Salt();
            var salt2 = _hashGenerator.Salt();

            // Assert
            Assert.AreNotEqual(salt, salt2);
        }
    }
}
