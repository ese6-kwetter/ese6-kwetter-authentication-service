using System;
using System.Threading.Tasks;
using Google.Apis.Auth;
using UserService.Entities;
using UserService.Exceptions;
using UserService.Helpers;
using UserService.Repositories;

namespace UserService.Services
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
        
        public async Task<User> LoginPasswordAsync(string email, string password)
        {
            var user = await _repository.ReadByEmailAsync(email);

            if (user == null)
                throw new EmailNotFoundException();
            
            if (!_hashGenerator.Verify(password, user.Salt, user.Password))
                throw new IncorrectPasswordException();

            user.JwtToken = _tokenGenerator.GenerateJwt(user.Id);
            
            return user.WithoutPassword();
        }

        public async Task<User> LoginGoogleAsync(string tokenId)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(
                tokenId, new GoogleJsonWebSignature.ValidationSettings()
            );
            
            if (payload == null)
                throw new GoogleAccountNotFoundException();

            var user = await _repository.ReadByEmailAsync(payload.Email);
            
            if (user == null)
                throw new GoogleAccountNotFoundException();

            user.JwtToken = _tokenGenerator.GenerateJwt(user.Id);

            return user.WithoutPassword();
        }

        public async Task<User> LoginAppleAsync(string tokenId)
        {
            throw new NotImplementedException();
        }
    }
}
