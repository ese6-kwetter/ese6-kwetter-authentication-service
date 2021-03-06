﻿using NUnit.Framework;
using UserMicroservice.Helpers;

namespace UserMicroserviceTests.Helpers
{
    [TestFixture]
    public class HashGeneratorTests
    {
        [SetUp]
        public void Setup()
        {
            _hashGenerator = new HashGenerator();
        }

        private IHashGenerator _hashGenerator;

        [Test]
        public void Hash_AreNotEqual_ReturnsTrue()
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
        public void Salt_GeneratesSalt_ReturnsByteArray()
        {
            // Arrange and act
            var salt = _hashGenerator.Salt();

            // Assert
            Assert.IsInstanceOf<byte[]>(salt);
        }

        [Test]
        public void Verify_IncorrectSalt_ReturnsFalse()
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
        public void Verify_IncorrectString_ReturnsFalse()
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
        public void Verify_IncorrectStringAndSalt_ReturnsFalse()
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
        public void Verify_ReturnsTrue()
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
    }
}
