using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace AuthenticationService.Helpers
{
    public class HashGenerator : IHashGenerator
    {
        public byte[] Salt()
        {
            // Generate a 128-bit salt using a secure PRNG
            var salt = new byte[16];
            
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            return salt;
        }

        public byte[] Hash(string plainText, byte[] salt)
        {
            // Derive a 256 bit subkey using HMACSHA256 with 8192 iterations
            return KeyDerivation.Pbkdf2(
                plainText,
                salt,
                KeyDerivationPrf.HMACSHA256,
                8192,
                256 / 8
            );
        }

        public bool Verify(string plainText, byte[] salt, byte[] hash)
        {
            var hashedPlainText = Hash(plainText, salt);
            
            return hash.SequenceEqual(hashedPlainText);
        }
    }
}
