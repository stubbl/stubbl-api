namespace Stubbl.Api.Core.Exceptions.MemberNotFound.Version1
{
   using System;
   using MongoDB.Bson;

   public class MemberNotFoundException : Exception
   {
      public MemberNotFoundException(ObjectId memberId, ObjectId teamId)
         : base($"Member not found. MemberID='{memberId}' TeamID='{teamId}'")
      {
         MemberId = memberId;
         TeamId = teamId;
      }

      public ObjectId TeamId { get; }
      public ObjectId MemberId { get; }
   }
}