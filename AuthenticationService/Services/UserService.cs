using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Entities;
using AuthenticationService.Helpers;
using AuthenticationService.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly IUserRepository _repository;
        
        public UserService(IOptions<AppSettings> appSettings, IUserRepository repository)
        {
            _appSettings = appSettings.Value;
            _repository = repository;
        }

        public async Task<User> Password(string username, string password)
        {
            // var users = await _repository.Get();
            // var user = users.SingleOrDefault(x => x.Username == username && x.Password == password);

            var user = new User { Id = new Guid(), Username = "test", Password = "test" };
            
            if (user == null)
                return null;

            user.JwtToken = GenerateJwtToken(user.Id.ToString());
            
            return user.WithoutPassword();
        }

        private string GenerateJwtToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, userId)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }
    }
}
