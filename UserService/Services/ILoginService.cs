using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Services
{
    public interface ILoginService
    {
        Task<User> LoginPasswordAsync(string email, string password);
        Task<User> LoginGoogleAsync(string tokenId);
        Task<User> LoginAppleAsync(string tokenId);
    }
}
