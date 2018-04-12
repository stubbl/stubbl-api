using System;

namespace Stubbl.Api.Exceptions.UserAlreadyExists.Version1
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string sub)
            : base($"User already exists. Sub='{sub}'")
        {
            Sub = sub;
        }

        public string Sub { get; }
    }
}