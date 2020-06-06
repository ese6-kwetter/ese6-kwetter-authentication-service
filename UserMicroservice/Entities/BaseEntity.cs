using System;
using MongoDB.Bson.Serialization.Attributes;

namespace UserMicroservice.Entities
{
    public abstract class BaseEntity
    {
        [BsonId] public Guid Id { get; set; }
    }
}
