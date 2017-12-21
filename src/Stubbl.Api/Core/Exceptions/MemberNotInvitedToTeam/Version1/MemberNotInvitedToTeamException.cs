namespace Stubbl.Api.Core.Exceptions.MemberNotInvitedToTeam.Version1
{
   using System;
   using MongoDB.Bson;

   public class MemberNotInvitedToTeamException : Exception
   {
      public MemberNotInvitedToTeamException(ObjectId memberId, ObjectId invitationId)
         : base($"Member not invited to the team. MemberID='{memberId}' InvitationID='{invitationId}'")
      {
         MemberId = memberId;
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
      public ObjectId MemberId { get; }
   }
}
