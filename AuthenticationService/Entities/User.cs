using System;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthenticationService.Entities
{
    public class User
    {
        [BsonId] public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string JwtToken { get; set; }
        public string OauthSubject { get; set; }
        public string OauthIssuer { get; set; }
    }
}
