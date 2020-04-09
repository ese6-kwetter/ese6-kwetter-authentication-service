using System;

namespace AuthenticationService.Helpers
{
    public interface ITokenGenerator
    {
        /// <summary>
        /// Generates a new JWT with user identity
        /// </summary>
        /// <param name="userId">Guid of the user</param>
        /// <returns>JWT as string</returns>
        string GenerateJwt(Guid userId);
    }
}
