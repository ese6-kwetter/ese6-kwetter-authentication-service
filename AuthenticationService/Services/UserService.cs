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
        private readonly IUserRepository _repository;
        private readonly ITokenGenerator _tokenGenerator;
        
        public UserService(IUserRepository repository, ITokenGenerator tokenGenerator)
        {
            _repository = repository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<User> Password(string username, string password)
        {
            //var users = await _repository.Read(username);
            //var user = users.SingleOrDefault(x => x.Username == username && x.Password == password);

            var user = new User { Id = new Guid(), Username = "test", Password = "test" };
            
            if (user == null)
                return null;

            user.JwtToken = _tokenGenerator.GenerateJwt(user.Id);
            
            return user.WithoutPassword();
        }

        public Task<User> Google()
        {
            throw new NotImplementedException();
        }

        public Task<User> Register(string username, string password, string email)
        {
            throw new NotImplementedException();
        }
    }
}
