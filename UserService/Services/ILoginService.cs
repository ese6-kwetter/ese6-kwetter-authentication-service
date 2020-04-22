using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Services
{
    public interface ILoginService
    {
        Task<User> LoginPassword(string email, string password);
        Task<User> LoginGoogle(string tokenId);
        Task<User> LoginApple(string tokenId);
    }
}
