namespace Stubbl.Api.Core.Exceptions.UserRoleNotFound.Version1
{
   using System;

   public class UserRoleNotFoundException : Exception
   {
      public UserRoleNotFoundException()
         : base("User role not found.")
      {
      }
   }
}
