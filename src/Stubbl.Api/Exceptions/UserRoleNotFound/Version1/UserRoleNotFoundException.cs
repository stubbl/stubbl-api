using System;

namespace Stubbl.Api.Exceptions.UserRoleNotFound.Version1
{
    public class UserRoleNotFoundException : Exception
    {
        public UserRoleNotFoundException()
            : base("User role not found.")
        {
        }
    }
}