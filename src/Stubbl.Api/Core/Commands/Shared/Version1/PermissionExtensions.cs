namespace Stubbl.Api.Core.Commands.Shared.Version1
{
   using System.Collections.Generic;
   using System.Linq;

   public static class PermissionExtensions
   {
      private static readonly Dictionary<Permission, Data.Collections.Shared.Permission> _dataMappings;
      private static readonly Dictionary<Permission, Events.Shared.Version1.Permission> _eventMappings;

      static PermissionExtensions()
      {
         _dataMappings = new Dictionary<Permission, Data.Collections.Shared.Permission>
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
            },
         };

         _eventMappings = new Dictionary<Permission, Events.Shared.Version1.Permission>
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
            },
         };
      }

      public static IReadOnlyCollection<Data.Collections.Shared.Permission> ToDataPermissions(this IReadOnlyCollection<Permission> extended)
      {
         return extended.Select(p => _dataMappings.ContainsKey(p) ? _dataMappings[p] : Data.Collections.Shared.Permission.Unknown)
            .ToList();
      }

      public static IReadOnlyCollection<Events.Shared.Version1.Permission> ToEventPermissions(this IReadOnlyCollection<Permission> extended)
      {
         return extended.Select(p => _eventMappings.ContainsKey(p) ? _eventMappings[p] : Events.Shared.Version1.Permission.Unknown)
            .ToList();
      }
   }
}