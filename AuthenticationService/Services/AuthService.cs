using System;
using System.Threading.Tasks;
using AuthenticationService.Entities;
using AuthenticationService.Helpers;
using AuthenticationService.Repositories;
using Google.Apis.Auth;

namespace AuthenticationService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repository;
        private readonly IHashGenerator _hashGenerator;
        private readonly ITokenGenerator _tokenGenerator;
        
        public AuthService(IUserRepository repository, IHashGenerator hashGenerator, ITokenGenerator tokenGenerator)
        {
            _repository = repository;
            _hashGenerator = hashGenerator;
            _tokenGenerator = tokenGenerator;
        }
        
        public async Task<User> LoginPassword(string email, string password)
        {
            throw new NotImplementedException();
            var user = await _repository.ReadByEmail(email);

            if (user == null)
                throw new ArgumentException("A user with this email does not exist.");
            
            if (!_hashGenerator.Verify(password, user.Salt, user.Password))
                throw new ArgumentException("The password is incorrect.");

            user.JwtToken = _tokenGenerator.GenerateJwt(user.Id);
            
            return user.WithoutPassword();
        }

        public async Task<User> LoginGoogle(string tokenId)
        {
            throw new NotImplementedException();
            var payload = await GoogleJsonWebSignature.ValidateAsync(
                tokenId, new GoogleJsonWebSignature.ValidationSettings()
            );
            
            if (payload == null)
                throw new ArgumentException("This Google account does not exist.");

            var user = await _repository.ReadByEmail(payload.Email);
            if (user == null)
                throw new ArgumentException("A user with this Google account does not exist.");

            user.JwtToken = _tokenGenerator.GenerateJwt(user.Id);

            return user.WithoutPassword();
        }

        public async Task<User> LoginApple(string tokenId)
        {
            throw new NotImplementedException();
        }
    }
}
