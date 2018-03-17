using System;

namespace Stubbl.Api.Exceptions.AdministratorRoleNotFound.Version1
{
    public class AdministratorRoleNotFoundException : Exception
    {
        public AdministratorRoleNotFoundException()
            : base("Administrator role not found.")
        {
        }
    }
}