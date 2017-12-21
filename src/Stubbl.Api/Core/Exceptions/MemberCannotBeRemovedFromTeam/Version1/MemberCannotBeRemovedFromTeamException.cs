namespace Stubbl.Api.Core.Exceptions.MemberCannotBeRemovedFromTeam.Version1
{
   using System;
   using MongoDB.Bson;

   public class MemberCannotBeRemovedFromTeamException : Exception
   {
      public MemberCannotBeRemovedFromTeamException(ObjectId memberId, ObjectId teamId)
         : base($"Member cannot be removed from the team. MemberID='{memberId}' TeamID='{teamId}'")
      {
         MemberId = memberId;
         TeamId = teamId;
      }

      public ObjectId TeamId { get; }
      public ObjectId MemberId { get; }
   }
}