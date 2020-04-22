using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Auth;
using UserService.Entities;

namespace UserService.Services
{
    public interface IRegisterService
    {
        Task<User> RegisterPassword(string username, string email, string password);
        Task<User> RegisterGoogle(string tokenId);
        Task<User> RegisterApple(string tokenId);
    }
}
