using System.Collections.Generic;
using System.Threading.Tasks;
using AuthenticationService.Entities;
using Google.Apis.Auth;

namespace AuthenticationService.Services
{
    public interface IUserService
    {
        Task<User> RegisterPassword(string email, string username, string password);
        Task<User> RegisterGoogle(string tokenId);
        Task<User> RegisterApple();
    }
}
