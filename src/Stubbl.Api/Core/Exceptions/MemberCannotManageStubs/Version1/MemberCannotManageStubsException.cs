namespace Stubbl.Api.Core.Exceptions.MemberCannotManageStubs.Version1
{
   using System;
   using MongoDB.Bson;

   public class MemberCannotManageStubsException : Exception
   {
      public MemberCannotManageStubsException(ObjectId memberId, ObjectId teamId)
         : base($"Member cannot manage stubs. MemberID='{memberId}' TeamID='{teamId}'")
      {
         MemberId = memberId;
         TeamId = teamId;
      }

      public ObjectId MemberId { get; }
      public ObjectId TeamId { get; }
   }
}