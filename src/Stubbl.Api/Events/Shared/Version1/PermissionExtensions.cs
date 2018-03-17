using System.Collections.Generic;
using System.Linq;

namespace Stubbl.Api.Events.Shared.Version1
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
                    Permission.ManageInvitations
                },
                {
                    Data.Collections.Shared.Permission.ManageMembers,
                    Permission.ManageMembers
                },
                {
                    Data.Collections.Shared.Permission.ManageRoles,
                    Permission.ManageRoles
                },
                {
                    Data.Collections.Shared.Permission.ManageTeams,
                    Permission.ManageTeams
                },
                {
                    Data.Collections.Shared.Permission.ManageStubs,
                    Permission.ManageStubs
                }
            };
        }

        public static IReadOnlyCollection<Data.Collections.Shared.Permission> ToDataPermissions(
            this IReadOnlyCollection<Permission> extended)
        {
            return extended.Select(p =>
                    s_mappings.Any(x => x.Value == p)
                        ? s_mappings.First(x => x.Value == p).Key
                        : Data.Collections.Shared.Permission.Unknown)
                .ToList();
        }

        public static IReadOnlyCollection<Permission> ToEventPermissions(
            this IReadOnlyCollection<Data.Collections.Shared.Permission> extended)
        {
            return extended.Select(p => s_mappings.ContainsKey(p) ? s_mappings[p] : Permission.Unknown)
                .ToList();
        }
    }
}