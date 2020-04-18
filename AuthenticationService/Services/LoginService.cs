using System;
using System.Threading.Tasks;
using AuthenticationService.Entities;
using AuthenticationService.Exceptions;
using AuthenticationService.Helpers;
using AuthenticationService.Repositories;
using Google.Apis.Auth;

namespace AuthenticationService.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _repository;
        private readonly IHashGenerator _hashGenerator;
        private readonly ITokenGenerator _tokenGenerator;
        
        public LoginService(IUserRepository repository, IHashGenerator hashGenerator, ITokenGenerator tokenGenerator)
        {
            _repository = repository;
            _hashGenerator = hashGenerator;
            _tokenGenerator = tokenGenerator;
        }
        
        public async Task<User> LoginPassword(string email, string password)
        {
            var user = await _repository.ReadByEmail(email);

            if (user == null)
                throw new EmailNotFoundException();
            
            if (!_hashGenerator.Verify(password, user.Salt, user.Password))
                throw new IncorrectPasswordException();

            user.JwtToken = _tokenGenerator.GenerateJwt(user.Id);
            
            return user.WithoutPassword();
        }

        public async Task<User> LoginGoogle(string tokenId)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(
                tokenId, new GoogleJsonWebSignature.ValidationSettings()
            );
            
            if (payload == null)
                throw new GoogleAccountNotFoundException();

            var user = await _repository.ReadByEmail(payload.Email);
            
            if (user == null)
                throw new GoogleAccountNotFoundException();

            user.JwtToken = _tokenGenerator.GenerateJwt(user.Id);

            return user.WithoutPassword();
        }

        public async Task<User> LoginApple(string tokenId)
        {
            throw new NotImplementedException();
        }
    }
}
