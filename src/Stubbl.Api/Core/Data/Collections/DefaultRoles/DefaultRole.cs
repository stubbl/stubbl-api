namespace Stubbl.Api.Core.Data.Collections.DefaultRoles
{
   using System.Collections.Generic;
   using MongoDB.Bson;
   using MongoDB.Bson.Serialization.Attributes;
   using Shared;

   public class DefaultRole
   {
      public DefaultRole()
      {
         Permissions = new Permission[0];
      }

      public ObjectId Id { get; set; }
      public string Name { get; set; }
      [BsonRepresentation(BsonType.String)]
      public IReadOnlyCollection<Permission> Permissions { get; set; }
   }
}