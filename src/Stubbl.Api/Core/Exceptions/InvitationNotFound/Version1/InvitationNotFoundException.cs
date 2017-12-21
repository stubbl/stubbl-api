namespace Stubbl.Api.Core.Exceptions.InvitationNotFound.Version1
{
   using System;
   using MongoDB.Bson;

   public class InvitationNotFoundException : Exception
   {
      public InvitationNotFoundException(ObjectId invitationId, ObjectId teamId)
         : base($"Invitation not found. InvitationID='{invitationId}' TeamID='{teamId}'")
      {
         InvitationId = invitationId;
         TeamId = teamId;
      }

      public ObjectId InvitationId { get; }
      public ObjectId TeamId { get; }
   }
}