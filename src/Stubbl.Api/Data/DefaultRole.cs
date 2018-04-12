using System.Collections.Generic;
using Stubbl.Api.Data.Collections.Shared;

namespace Stubbl.Api.Data
{
    public class DefaultRole
    {
        public DefaultRole(string name, IReadOnlyCollection<Permission> permissions = null)
        {
            Name = name;
            Permissions = permissions ?? new Permission[0];
        }

        public string Name { get; }
        public IReadOnlyCollection<Permission> Permissions { get; }
    }
}