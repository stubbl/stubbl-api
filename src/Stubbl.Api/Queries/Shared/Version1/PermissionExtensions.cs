using System.Collections.Generic;

namespace Stubbl.Api.Queries.Shared.Version1
{
    public static class PermissionExtensions
    {
        private static readonly Dictionary<Data.Collections.Shared.Permission, Permission> s_mappings;

        static PermissionExtensions()
        {
            s_mappings = new Dictionary<Data.Collections.Shared.Permission, Permission>
            {
                {
                    Data.Collections.Shared.Permission.ManageInvitations,
                    new Permission("Manage invitations", "Create/update/delete invitations.")
                },
                {
                    Data.Collections.Shared.Permission.ManageMembers,
                    new Permission("Manage members", "Add/remove members.")
                },
                {
                    Data.Collections.Shared.Permission.ManageRoles,
                    new Permission("Manage roles", "Create/update/delete roles.")
                },
                {
                    Data.Collections.Shared.Permission.ManageTeams,
                    new Permission("Manage teams", "Create/update/delete teams.")
                },
                {
                    Data.Collections.Shared.Permission.ManageStubs,
                    new Permission("Manage stubs", "Create/update/delete stubs.")
                }
            };
        }

        public static Permission ToQueryPermission(this Data.Collections.Shared.Permission extended)
        {
            return s_mappings.ContainsKey(extended) ? s_mappings[extended] : null;
        }
    }
}