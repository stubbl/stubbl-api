using System.Collections.Generic;
using System.Linq;

namespace Stubbl.Api.Commands.Shared.Version1
{
    public static class PermissionExtensions
    {
        private static readonly Dictionary<Permission, Data.Collections.Shared.Permission> s_dataMappings;
        private static readonly Dictionary<Permission, Events.Shared.Version1.Permission> s_eventMappings;

        static PermissionExtensions()
        {
            s_dataMappings = new Dictionary<Permission, Data.Collections.Shared.Permission>
            {
                {
                    Permission.ManageInvitations,
                    Data.Collections.Shared.Permission.ManageInvitations
                },
                {
                    Permission.ManageMembers,
                    Data.Collections.Shared.Permission.ManageMembers
                },
                {
                    Permission.ManageRoles,
                    Data.Collections.Shared.Permission.ManageRoles
                },
                {
                    Permission.ManageStubs,
                    Data.Collections.Shared.Permission.ManageStubs
                },
                {
                    Permission.ManageTeams,
                    Data.Collections.Shared.Permission.ManageTeams
                }
            };

            s_eventMappings = new Dictionary<Permission, Events.Shared.Version1.Permission>
            {
                {
                    Permission.ManageInvitations,
                    Events.Shared.Version1.Permission.ManageInvitations
                },
                {
                    Permission.ManageMembers,
                    Events.Shared.Version1.Permission.ManageMembers
                },
                {
                    Permission.ManageRoles,
                    Events.Shared.Version1.Permission.ManageRoles
                },
                {
                    Permission.ManageStubs,
                    Events.Shared.Version1.Permission.ManageStubs
                },
                {
                    Permission.ManageTeams,
                    Events.Shared.Version1.Permission.ManageTeams
                }
            };
        }

        public static IReadOnlyCollection<Data.Collections.Shared.Permission> ToDataPermissions(
            this IReadOnlyCollection<Permission> extended)
        {
            return extended.Select(p =>
                    s_dataMappings.ContainsKey(p) ? s_dataMappings[p] : Data.Collections.Shared.Permission.Unknown)
                .ToList();
        }

        public static IReadOnlyCollection<Events.Shared.Version1.Permission> ToEventPermissions(
            this IReadOnlyCollection<Permission> extended)
        {
            return extended.Select(p =>
                    s_eventMappings.ContainsKey(p) ? s_eventMappings[p] : Events.Shared.Version1.Permission.Unknown)
                .ToList();
        }
    }
}