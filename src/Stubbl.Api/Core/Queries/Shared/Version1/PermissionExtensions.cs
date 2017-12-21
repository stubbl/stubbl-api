namespace Stubbl.Api.Core.Queries.Shared.Version1
{
   using System.Collections.Generic;

   public static class PermissionExtensions
   {
      private static readonly Dictionary<Data.Collections.Shared.Permission, Permission> _mappings;

      static PermissionExtensions()
      {
         _mappings = new Dictionary<Data.Collections.Shared.Permission, Permission>
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
         return _mappings.ContainsKey(extended) ? _mappings[extended] : null;
      }
   }
}