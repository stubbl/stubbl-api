namespace Stubbl.Api.Core.Events.TeamCreated.Version1
{
   using System.Collections.Generic;
   using MongoDB.Bson;
   using Shared.Version1;

   public class Role
   {
      public Role(ObjectId id, string name, IReadOnlyCollection<Permission> permissions)
      {
         Id = id;
         Name = name;
         Permissions = permissions;
      }

      public ObjectId Id { get; }
      public string Name { get; }
      public IReadOnlyCollection<Permission> Permissions { get; }
   }
}