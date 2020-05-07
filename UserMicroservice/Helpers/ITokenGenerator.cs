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
    }
}
