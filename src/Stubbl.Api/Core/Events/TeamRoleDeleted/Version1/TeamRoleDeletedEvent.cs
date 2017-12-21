namespace Stubbl.Api.Core.Events.TeamRoleDeleted.Version1
{
   using Common.Events;
   using MongoDB.Bson;

   public class TeamRoleDeletedEvent : IEvent
   {
      public TeamRoleDeletedEvent(ObjectId roleId, ObjectId teamId)
      {
         RoleId = roleId;
         TeamId = teamId;
      }

      public ObjectId RoleId { get; }
      public ObjectId TeamId { get; }
   }
}
