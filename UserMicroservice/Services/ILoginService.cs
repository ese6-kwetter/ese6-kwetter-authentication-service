using System.Threading.Tasks;
using UserMicroservice.Entities;

namespace UserMicroservice.Services
{
    public interface ILoginService
    {
        Task<User> LoginPasswordAsync(string email, string password);
        Task<User> LoginGoogleAsync(string tokenId);
        Task<User> LoginAppleAsync(string tokenId);
    }
}
