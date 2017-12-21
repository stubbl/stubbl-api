namespace Stubbl.Api.Core.Exceptions.MemberCannotManageTeams.Version1
{
   using System;
   using MongoDB.Bson;

   public class MemberCannotManageTeamsException : Exception
   {
      public MemberCannotManageTeamsException(ObjectId memberId, ObjectId teamId)
         : base($"Member cannot manage teams. MemberID='{memberId}' TeamID='{teamId}'")
      {
         MemberId = memberId;
         TeamId = teamId;
      }

      public ObjectId MemberId { get; }
      public ObjectId TeamId { get; }
   }
}