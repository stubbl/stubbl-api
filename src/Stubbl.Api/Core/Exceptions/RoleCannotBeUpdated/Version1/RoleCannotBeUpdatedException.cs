namespace Stubbl.Api.Core.Exceptions.RoleCannotBeUpdated.Version1
{
   using System;
   using MongoDB.Bson;

   public class RoleCannotBeUpdatedException : Exception
   {
      public RoleCannotBeUpdatedException(ObjectId roleId, ObjectId teamId)
         : base($"Role cannot be updated. RoleID='{roleId}' TeamID='{teamId}'")
      {
         RoleId = roleId;
         TeamId = teamId;
      }

      public ObjectId RoleId { get; }
      public ObjectId TeamId { get; }
   }
}