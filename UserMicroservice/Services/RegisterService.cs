using System;
using System.Threading.Tasks;
using Google.Apis.Auth;
using UserMicroservice.Entities;
using UserMicroservice.Exceptions;
using UserMicroservice.Helpers;
using UserMicroservice.Repositories;

namespace UserMicroservice.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _repository;
        private readonly IRegexValidator _regexValidator;
        private readonly IHashGenerator _hashGenerator;
        
        public RegisterService(IUserRepository repository, IRegexValidator regexValidator, IHashGenerator hashGenerator)
        {
            _repository = repository;
            _regexValidator = regexValidator;
            _hashGenerator = hashGenerator;
        }

        public async Task<User> RegisterPasswordAsync(string username, string email, string password)
        {
            if (!_regexValidator.IsValidEmail(email))
                throw new InvalidEmailException();
            
            if (!_regexValidator.IsValidPassword(password))
                throw new InvalidPasswordException();
                
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
            });

            return user.WithoutSensitiveData();
        }

        public async Task<User> RegisterGoogleAsync(string tokenId)
        {
            var payload =
                await GoogleJsonWebSignature.ValidateAsync(tokenId, new GoogleJsonWebSignature.ValidationSettings())
                ?? throw new GoogleAccountNotFoundException();

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

            return user.WithoutSensitiveData();
        }

        public async Task<User> RegisterAppleAsync(string tokenId)
        {
            throw new NotImplementedException();
        }
    }
}
