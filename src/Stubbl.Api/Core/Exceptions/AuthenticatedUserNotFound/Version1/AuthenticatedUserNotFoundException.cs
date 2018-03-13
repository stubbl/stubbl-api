namespace Stubbl.Api.Core.Exceptions.AuthenticatedUserNotFound.Version1
{
   using System;

   public class AuthenticatedUserNotFoundException : Exception
   {
      public AuthenticatedUserNotFoundException(string identityId)
         : base($"Authenticated user not found. IdentityID='{identityId}'")
      {
         IdentityId = identityId;
      }

      public string IdentityId { get; }
   }
}