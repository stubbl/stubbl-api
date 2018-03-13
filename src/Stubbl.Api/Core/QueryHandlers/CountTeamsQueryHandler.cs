namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.QueryHandlers;
   using Core.Authentication;
   using Data.Collections.Teams;
   using MongoDB.Driver;
   using Queries.CountTeams.Version1;

   public class CountTeamsQueryHandler : IQueryHandler<CountTeamsQuery, CountTeamsProjection>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Team> _teamsCollection;

      public CountTeamsQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Team> teamsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _teamsCollection = teamsCollection;
      }

      public Task<CountTeamsProjection> HandleAsync(CountTeamsQuery query, CancellationToken cancellationToken)
      {
         var projection = new CountTeamsProjection
         (
            _authenticatedUserAccessor.AuthenticatedUser.Teams.Count
         );

         return Task.FromResult(projection);
      }
   }
}
