using MongoDB.Bson.Serialization.Attributes;

namespace UserMicroservice.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        [BsonIgnore] public string Token { get; set; }
        public string OAuthSubject { get; set; }
        public string OAuthIssuer { get; set; }
    }
}
