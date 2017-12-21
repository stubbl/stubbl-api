namespace Stubbl.Api.Core.Exceptions.MemberAlreadyInvitedToTeam.Version1
{
   using System;
   using MongoDB.Bson;

   public class MemberAlreadyInvitedToTeamException : Exception
   {
      public MemberAlreadyInvitedToTeamException(ObjectId memberId, ObjectId teamId)
         : base($"Member has already been invited to the team. MemberID='{memberId}' TeamID='{teamId}'")
      {
         MemberId = memberId;
         TeamId = teamId;
      }

      public ObjectId MemberId { get; }
      public ObjectId TeamId { get; }
   }
}