namespace Stubbl.Api.Core.Events.TeamRoleCreated.Version1
{
   using System.Collections.Generic;
   using Events.Shared.Version1;
   using Common.Events;
   using MongoDB.Bson;

   public class TeamRoleCreatedEvent : IEvent
   {
      public TeamRoleCreatedEvent(ObjectId roleId, ObjectId teamId, string name, IReadOnlyCollection<Permission> permissions)
      {
         RoleId = roleId;
         TeamId = teamId;
         Name = name;
         Permissions = permissions;
      }

      public string Name { get; }
      public ObjectId RoleId { get; }
      public IReadOnlyCollection<Permission> Permissions { get; }
      public ObjectId TeamId { get; }
   }
}
