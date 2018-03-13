namespace Stubbl.Api.Core.QueryHandlers
{
   using System;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Common.QueryHandlers;
   using Queries.FindAuthenticatedUser.Version1;
   using Queries.Shared.Version1;

   public class FindAuthenticatedUserQueryHandler : IQueryHandler<FindAuthenticatedUserQuery, FindAuthenticatedUserProjection>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;

      public FindAuthenticatedUserQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
      }

      public Task<FindAuthenticatedUserProjection> HandleAsync(FindAuthenticatedUserQuery query, CancellationToken cancellationToken)
      {
         return Task.FromResult(new FindAuthenticatedUserProjection
         (
            _authenticatedUserAccessor.AuthenticatedUser.Id.ToString(),
            _authenticatedUserAccessor.AuthenticatedUser.Name,
            _authenticatedUserAccessor.AuthenticatedUser.EmailAddress,
            _authenticatedUserAccessor.AuthenticatedUser.Teams.Select(t => new Team
               (
                  t.Id.ToString(),
                  t.Name,
                  new Role
                  (
                     t.Role.Id.ToString(),
                     t.Role.Name,
                     t.Role.Permissions.Select(p => p.ToQueryPermission()).Where(p => p != null).ToList()
                  )
               ))
               .ToList()
         ));
      }
   }
}
