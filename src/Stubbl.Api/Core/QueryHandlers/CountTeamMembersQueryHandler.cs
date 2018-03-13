namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Common.QueryHandlers;
   using Core.Authentication;
   using Core.Exceptions.MemberNotAddedToTeam.Version1;
   using Data.Collections.Teams;
   using MongoDB.Driver;
   using Queries.CountTeamMembers.Version1;

   public class CountTeamMembersQueryHandler : IQueryHandler<CountTeamMembersQuery, CountTeamMembersProjection>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Team> _teamsCollection;

      public CountTeamMembersQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Team> teamsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _teamsCollection = teamsCollection;
      }

      public async Task<CountTeamMembersProjection> HandleAsync(CountTeamMembersQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
               query.TeamId
            );
         }

         var totalCount = await _teamsCollection.Find(t => t.Id == query.TeamId)
            .Project(t => t.Members.Count())
            .SingleOrDefaultAsync();

         return new CountTeamMembersProjection
         (
            totalCount
         );
      }
   }
}
