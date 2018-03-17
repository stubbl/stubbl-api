namespace Stubbl.Api.Core.QueryHandlers
{
   using System;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Gunnsoft.Cqs.QueryHandlers;
   using Core.Commands.Shared.Version1;
   using Queries.CountPermissions.Version1;

   public class CountPermissionsQueryHandler : IQueryHandler<CountPermissionsQuery, CountPermissionsProjection>
   {
      public Task<CountPermissionsProjection> HandleAsync(CountPermissionsQuery query, CancellationToken cancellationToken)
      {
         var totalCount = Enum.GetValues(typeof(Permission))
            .Cast<Permission>()
            .Count();

         var projection = new CountPermissionsProjection
         (
            totalCount
         );

         return Task.FromResult(projection);
      }
   }
}
