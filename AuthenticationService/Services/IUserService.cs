using System.Collections.Generic;
using System.Threading.Tasks;
using AuthenticationService.Entities;
using Google.Apis.Auth;

namespace AuthenticationService.Services
{
    public interface IUserService
    {
        Task<User> Password(string username, string password);
        //Task<User> Authenticate(GoogleJsonWebSignature.Payload payload);
    }
}
