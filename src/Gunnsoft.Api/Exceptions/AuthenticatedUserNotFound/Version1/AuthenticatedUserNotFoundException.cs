using System;

namespace Gunnsoft.Api.Exceptions.AuthenticatedUserNotFound.Version1
{
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