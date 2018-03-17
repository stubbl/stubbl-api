using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Stubbl.Api.Data.Collections.Shared;

namespace Stubbl.Api.Data.Collections.DefaultRoles
{
    public class DefaultRole
    {
        public DefaultRole()
        {
            Permissions = new Permission[0];
        }

        public ObjectId Id { get; set; }
        public string Name { get; set; }

        [BsonRepresentation(BsonType.String)] public IReadOnlyCollection<Permission> Permissions { get; set; }
    }
}