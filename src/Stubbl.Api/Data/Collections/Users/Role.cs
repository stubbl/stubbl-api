using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Stubbl.Api.Data.Collections.Shared;

namespace Stubbl.Api.Data.Collections.Users
{
    public class Role
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }

        [BsonRepresentation(BsonType.String)] public IReadOnlyCollection<Permission> Permissions { get; set; }
    }
}