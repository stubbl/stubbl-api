namespace Stubbl.Api.Core.QueryHandlers
{
   using System;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Common.QueryHandlers;
   using Queries.FindAuthenticatedMember.Version1;
   using Queries.Shared.Version1;

   public class FindAuthenticatedMemberQueryHandler : IQueryHandler<FindAuthenticatedMemberQuery, FindAuthenticatedMemberProjection>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;

      public FindAuthenticatedMemberQueryHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
      }

      public Task<FindAuthenticatedMemberProjection> HandleAsync(FindAuthenticatedMemberQuery query, CancellationToken cancellationToken)
      {
         return Task.FromResult(new FindAuthenticatedMemberProjection
         (
            _authenticatedMemberAccessor.AuthenticatedMember.Id.ToString(),
            _authenticatedMemberAccessor.AuthenticatedMember.Name,
            _authenticatedMemberAccessor.AuthenticatedMember.EmailAddress,
            _authenticatedMemberAccessor.AuthenticatedMember.Teams.Select(t => new Team
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
