namespace Stubbl.Api.Core.Exceptions.RoleNotFound.Version1
{
   using System;
   using MongoDB.Bson;

   public class RoleNotFoundException : Exception
   {
      public RoleNotFoundException(ObjectId roleId, ObjectId teamId)
         : base($"Role not found. RoleID='{roleId}' TeamID='{teamId}'")
      {
         RoleId = roleId;
         TeamId = teamId;
      }

      public ObjectId RoleId { get; }
      public ObjectId TeamId { get; }
   }
}