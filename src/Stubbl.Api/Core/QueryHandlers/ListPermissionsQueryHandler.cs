namespace Stubbl.Api.Core.QueryHandlers
{
   using System;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Gunnsoft.Cqs.QueryHandlers;
   using Queries.ListPermissions.Version1;
   using Queries.Shared.Version1;
   using Permission = Data.Collections.Shared.Permission;

   public class ListPermissionsQueryHandler : IQueryHandler<ListPermissionsQuery, ListPermissionsProjection>
   {
      public Task<ListPermissionsProjection> HandleAsync(ListPermissionsQuery query, CancellationToken cancellationToken)
      {
         return Task.FromResult(new ListPermissionsProjection
         (
            Enum.GetValues(typeof(Permission))
               .Cast<Permission>()
               .Where(p => p > 0)
               .Select(p => p.ToQueryPermission())
               .Where(p => p != null)
               .ToList()
         ));
      }
   }
}