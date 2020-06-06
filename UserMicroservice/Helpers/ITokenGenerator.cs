using System;

namespace UserMicroservice.Helpers
{
    public interface ITokenGenerator
    {
        /// <summary>
        ///     Generates a new JSON Web Token with the user identity.
        /// </summary>
        /// <param name="userId">Guid of the user</param>
        /// <returns>JSON Web Token</returns>
        string GenerateJwt(Guid userId);

        /// <summary>
        ///     Validates a JSON Web Token with claim
        /// </summary>
        /// <param name="token">Token of the Bearer</param>
        /// <param name="claim">Claim to validate with</param>
        /// <returns>JSON Web Token</returns>
        bool ValidateJwt(string token, string claim);
    }
}
