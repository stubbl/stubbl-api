using System;

namespace Gunnsoft.Api.Exceptions.AuthenticatedUserNotFound.Version1
{
    public class AuthenticatedUserNotFoundException : Exception
    {
        public AuthenticatedUserNotFoundException(string sub)
            : base($"Authenticated user not found. Sub='{sub}'")
        {
            Sub = sub;
        }

        public string Sub { get; }
    }
}