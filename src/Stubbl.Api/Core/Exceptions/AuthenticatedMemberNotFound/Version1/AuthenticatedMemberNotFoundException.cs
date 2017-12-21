namespace Stubbl.Api.Core.Exceptions.AuthenticatedMemberNotFound.Version1
{
   using System;

   public class AuthenticatedMemberNotFoundException : Exception
   {
      public AuthenticatedMemberNotFoundException(string identityId)
         : base($"Authenticated member not found. IdentityID='{identityId}'")
      {
         IdentityId = identityId;
      }

      public string IdentityId { get; }
   }
}