namespace Stubbl.Api.Core.Events.TeamInvitationCreated.Version1
{
   using Gunnsoft.Cqs.Events;
   using MongoDB.Bson;

   public class TeamInvitationCreatedEvent : IEvent
   {
      public TeamInvitationCreatedEvent(ObjectId invitationId, ObjectId teamId, ObjectId roleId, string emailAddress)
      {
         InvitationId = invitationId;
         TeamId = teamId;
         RoleId = roleId;
         EmailAddress = emailAddress;
      }

      public string EmailAddress { get; }
      public ObjectId InvitationId { get; }
      public ObjectId RoleId { get; }
      public ObjectId TeamId { get; }
   }
}