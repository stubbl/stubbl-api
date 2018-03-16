namespace Stubbl.Api.Core.Queries.ListPermissions.Version1
{
   using System.Collections.Generic;
   using CodeContrib.Queries;
   using Shared.Version1;

   public class ListPermissionsProjection : IProjection
   {
      public ListPermissionsProjection(IReadOnlyCollection<Permission> permissions)
      {
         Permissions = permissions;
      }

      public IReadOnlyCollection<Permission> Permissions { get; }
   }
}