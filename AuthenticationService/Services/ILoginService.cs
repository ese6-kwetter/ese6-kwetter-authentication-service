using System.Threading.Tasks;
using AuthenticationService.Entities;

namespace AuthenticationService.Services
{
    public interface ILoginService
    {
        Task<User> LoginPassword(string email, string password);
        Task<User> LoginGoogle(string tokenId);
        Task<User> LoginApple(string tokenId);
    }
}
