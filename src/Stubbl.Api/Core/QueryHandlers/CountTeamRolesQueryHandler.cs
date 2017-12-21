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
   using Queries.CountTeamRoles.Version1;

   public class CountTeamRolesQueryHandler : IQueryHandler<CountTeamRolesQuery, CountTeamRolesProjection>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<Team> _teamsCollection;

      public CountTeamRolesQueryHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<Team> teamsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _teamsCollection = teamsCollection;
      }

      public async Task<CountTeamRolesProjection> HandleAsync(CountTeamRolesQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedMemberAccessor.AuthenticatedMember.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedMemberAccessor.AuthenticatedMember.Id,
               query.TeamId
            );
         }

         var totalCount = await _teamsCollection.Find(t => t.Id == query.TeamId)
            .Project(t => t.Roles.Count())
            .SingleOrDefaultAsync();

         return new CountTeamRolesProjection
         (
            totalCount
         );
      }
   }
}
