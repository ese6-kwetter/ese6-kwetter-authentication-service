using System;
using System.Threading.Tasks;
using Google.Apis.Auth;
using MessageBroker;
using Newtonsoft.Json;
using UserMicroservice.Entities;
using UserMicroservice.Exceptions;
using UserMicroservice.Helpers;
using UserMicroservice.Repositories;

namespace UserMicroservice.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IHashGenerator _hashGenerator;
        private readonly IMessageQueuePublisher _messageQueuePublisher;
        private readonly IRegexValidator _regexValidator;
        private readonly IUserRepository _repository;

        public RegisterService(
            IUserRepository repository,
            IRegexValidator regexValidator,
            IHashGenerator hashGenerator,
            IMessageQueuePublisher messageQueuePublisher
        )
        {
            _repository = repository;
            _regexValidator = regexValidator;
            _hashGenerator = hashGenerator;
            _messageQueuePublisher = messageQueuePublisher;
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

            var user = await _repository.CreateAsync(
                new User
                {
                    Username = username,
                    Email = email,
                    Password = hashedPassword,
                    Salt = salt
                }
            );

            await _messageQueuePublisher.PublishMessageAsync(
                "Dwetter",
                "EmailMicroservice",
                "RegisterUser",
                new
                {
                    email = user.Email,
                    username = user.Username
                }
            );

            await _messageQueuePublisher.PublishMessageAsync(
                "Dwetter",
                "ProfileMicroservice",
                "RegisterUser",
                new
                {
                    userId = user.Id,
                    username = user.Username
                }
            );

            return user.WithoutSensitiveData();
        }

        public async Task<User> RegisterGoogleAsync(string token)
        {
            var payload =
                await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings())
                ?? throw new AccountNotFoundException();

            // Check if user already exists
            var user = await _repository.ReadByEmailAsync(payload.Email);

            if (user != null)
            {
                if (user.OAuthIssuer == "Google")
                    throw new AccountAlreadyExistsException();

                throw new EmailAlreadyExistsException();
            }

            user = await _repository.CreateAsync(
                new User
                {
                    Username = payload.Name,
                    Email = payload.Email,
                    OAuthIssuer = "Google",
                    OAuthSubject = payload.Subject
                }
            );

            // Publish message to EmailMicroservice queue
            await _messageQueuePublisher.PublishMessageAsync(
                "Dwetter",
                "EmailMicroservice",
                "RegisterUser",
                new
                {
                    email = user.Email,
                    username = user.Username
                }
            );

            // Publish message to ProfileMicroservice queue
            await _messageQueuePublisher.PublishMessageAsync(
                "Dwetter",
                "ProfileMicroservice",
                "RegisterUser",
                new
                {
                    userId = user.Id,
                    username = user.Username
                }
            );

            return user.WithoutSensitiveData();
        }

        public async Task<User> RegisterAppleAsync(string tokenId)
        {
            throw new NotImplementedException();
        }
    }
}
