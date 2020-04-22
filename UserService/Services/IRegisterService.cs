using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Auth;
using UserService.Entities;

namespace UserService.Services
{
    public interface IRegisterService
    {
        Task<User> RegisterPasswordAsync(string username, string email, string password);
        Task<User> RegisterGoogleAsync(string tokenId);
        Task<User> RegisterAppleAsync(string tokenId);
    }
}
