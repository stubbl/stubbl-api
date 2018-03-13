namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Common.QueryHandlers;
   using Queries.ListTeams.Version1;

   public class ListTeamsQueryHandler : IQueryHandler<ListTeamsQuery, ListTeamsProjection>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;

      public ListTeamsQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
      }

      public Task<ListTeamsProjection> HandleAsync(ListTeamsQuery query, CancellationToken cancellationToken)
      {
         return Task.FromResult(new ListTeamsProjection
         (
            _authenticatedUserAccessor.AuthenticatedUser.Teams.Select(t => new Team
               (
                  t.Id.ToString(),
                  t.Name
               ))
               .ToList()
         ));
      }
   }
}