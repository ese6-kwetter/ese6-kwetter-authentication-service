using System;
using MongoDB.Bson.Serialization.Attributes;

namespace UserService.Entities
{
    public class User
    {
        [BsonId] public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public string JwtToken { get; set; }
        public string OAuthSubject { get; set; }
        public string OAuthIssuer { get; set; }
    }
}
