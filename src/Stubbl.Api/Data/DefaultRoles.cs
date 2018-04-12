using System;
using System.Linq;
using Stubbl.Api.Data.Collections.Shared;

namespace Stubbl.Api.Data
{
    public static class DefaultRoles
    {
        static DefaultRoles()
        {
            Administrator = new DefaultRole
            (
                "Administrator",
                Enum.GetValues(typeof(Permission))
                    .Cast<Permission>()
                    .Where(p => p > 0)
                    .ToList()
            );

            User = new DefaultRole
            (
                "User"
            );
        }

        public static DefaultRole Administrator { get; }
        public static DefaultRole User { get; }
    }
}