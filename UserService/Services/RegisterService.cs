using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Entities;
using UserService.Exceptions;
using UserService.Helpers;
using UserService.Repositories;

namespace UserService.Services
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

        public async Task<User> RegisterPasswordAsync(string username, string email, string password)
        {
            if (await _repository.ReadByUsernameAsync(username) != null)
                throw new UsernameAlreadyExistsException();

            if (await _repository.ReadByEmailAsync(email) != null)
                throw new EmailAlreadyExistsException();

            var salt = _hashGenerator.Salt();
            var hashedPassword = _hashGenerator.Hash(password, salt);

            var user = await _repository.CreateAsync(new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword,
                Salt = salt,
                OAuthIssuer = "none"
            });

            return user.WithoutPassword();
        }

        public async Task<User> RegisterGoogleAsync(string tokenId)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(
                tokenId, new GoogleJsonWebSignature.ValidationSettings()
            );

            // Check if user already exists
            var user = await _repository.ReadByEmailAsync(payload.Email);
            
            if (user != null)
            {
                if (user.OAuthIssuer == "Google")
                    throw new GoogleAccountAlreadyExistsException();
                throw new EmailAlreadyExistsException();
            }

            user = await _repository.CreateAsync(new User
            {
                Username = payload.Name,
                Email = payload.Email,
                OAuthIssuer = "Google",
                OAuthSubject = payload.Subject
            });

            return user.WithoutPassword();
        }

        public async Task<User> RegisterAppleAsync(string tokenId)
        {
            throw new NotImplementedException();
        }
    }
}
