namespace Stubbl.Api.Core.Events.Shared.Version1
{
   using System.Collections.Generic;
   using System.Linq;

   public static class PermissionExtensions
   {
      private static readonly Dictionary<Data.Collections.Shared.Permission, Permission> _mappings;

      static PermissionExtensions()
      {
         _mappings = new Dictionary<Data.Collections.Shared.Permission, Permission>
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

      public static IReadOnlyCollection<Data.Collections.Shared.Permission> ToDataPermissions(this IReadOnlyCollection<Permission> extended)
      {
         return extended.Select(p => _mappings.Any(x => x.Value == p) ? _mappings.First(x => x.Value == p).Key : Data.Collections.Shared.Permission.Unknown)
            .ToList();
      }

      public static IReadOnlyCollection<Permission> ToEventPermissions(this IReadOnlyCollection<Data.Collections.Shared.Permission> extended)
      {
         return extended.Select(p => _mappings.ContainsKey(p) ? _mappings[p] : Permission.Unknown)
            .ToList();
      }
   }
}