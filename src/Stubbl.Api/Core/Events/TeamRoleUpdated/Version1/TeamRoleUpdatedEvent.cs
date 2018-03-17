namespace Stubbl.Api.Core.Events.TeamRoleUpdated.Version1
{
   using System.Collections.Generic;
   using Gunnsoft.Cqs.Events;
   using MongoDB.Bson;
   using Shared.Version1;

   public class TeamRoleUpdatedEvent : IEvent
   {
      public TeamRoleUpdatedEvent(ObjectId teamId, ObjectId roleId, string name, IReadOnlyCollection<Permission> permissions)
      {
         TeamId = teamId;
         RoleId = roleId;
         Name = name;
         Permissions = permissions;
      }

      public string Name { get; }
      public IReadOnlyCollection<Permission> Permissions { get; }
      public ObjectId RoleId { get; }
      public ObjectId TeamId { get; }
   }
}