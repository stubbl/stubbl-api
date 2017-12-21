namespace Stubbl.Api.Core.Exceptions.InvitationAlreadyUsed.Version1
{
   using System;
   using MongoDB.Bson;

   public class InvitationAlreadyUsedException : Exception
   {
      public InvitationAlreadyUsedException(ObjectId invitationId, ObjectId teamId)
         : base($"Invitation already used. InvitationID='{invitationId}' TeamID='{teamId}'")
      {
         TeamId = teamId;
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
      public ObjectId TeamId { get; }
   }
}