using System.Threading.Tasks;
using UserMicroservice.Entities;

namespace UserMicroservice.Services
{
    public interface IRegisterService
    {
        Task<User> RegisterPasswordAsync(string username, string email, string password);
        Task<User> RegisterGoogleAsync(string token);
        Task<User> RegisterAppleAsync(string tokenId);
    }
}
