using System.Collections.Generic;
using System.Threading.Tasks;
using AuthenticationService.Entities;
using Google.Apis.Auth;

namespace AuthenticationService.Services
{
    public interface IRegisterService
    {
        Task<User> RegisterPassword(string username, string email, string password);
        Task<User> RegisterGoogle(string tokenId);
        Task<User> RegisterApple(string tokenId);
    }
}
