namespace Stubbl.Api.Core.Exceptions.MemberCannotManageRoles.Version1
{
   using System;
   using MongoDB.Bson;

   public class MemberCannotManageRolesException : Exception
   {
      public MemberCannotManageRolesException(ObjectId memberId, ObjectId teamId)
         : base($"AuthenticatedUser cannot manage roles. MemberID='{memberId}' TeamID='{teamId}'")
      {
         MemberId = memberId;
         TeamId = teamId;
      }

      public ObjectId MemberId { get; }
      public ObjectId TeamId { get; }
   }
}