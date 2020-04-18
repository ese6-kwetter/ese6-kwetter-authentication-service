using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Entities;
using AuthenticationService.Exceptions;
using AuthenticationService.Helpers;
using AuthenticationService.Repositories;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _repository;
        private readonly IHashGenerator _hashGenerator;
        
        public RegisterService(IUserRepository repository, IHashGenerator hashGenerator)
        {
            _repository = repository;
            _hashGenerator = hashGenerator;
        }

        public async Task<User> RegisterPassword(string username, string email, string password)
        {
            if (await _repository.ReadByUsername(username) != null)
                throw new UsernameAlreadyExistsException();

            if (await _repository.ReadByEmail(email) != null)
                throw new EmailAlreadyExistsException();

            var salt = _hashGenerator.Salt();
            var hashedPassword = _hashGenerator.Hash(password, salt);

            var user = await _repository.Create(new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword,
                Salt = salt,
                OAuthIssuer = "none"
            });

            return user.WithoutPassword();
        }

        public async Task<User> RegisterGoogle(string tokenId)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(
                tokenId, new GoogleJsonWebSignature.ValidationSettings()
            );

            // Check if user already exists
            var user = await _repository.ReadByEmail(payload.Email);
            
            if (user != null)
            {
                if (user.OAuthIssuer == "Google")
                    throw new GoogleAccountAlreadyExistsException();
                throw new EmailAlreadyExistsException();
            }

            user = await _repository.Create(new User
            {
                Username = payload.Name,
                Email = payload.Email,
                OAuthIssuer = "Google",
                OAuthSubject = payload.Subject
            });

            return user.WithoutPassword();
        }

        public async Task<User> RegisterApple(string tokenId)
        {
            throw new NotImplementedException();
        }
    }
}
