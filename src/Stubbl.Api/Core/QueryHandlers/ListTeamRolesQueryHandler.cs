namespace Stubbl.Api.Core.QueryHandlers
{
   using System;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using CodeContrib.QueryHandlers;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using MongoDB.Driver;
   using Queries.ListTeamRoles.Version1;
   using Queries.Shared.Version1;
   using Team = Data.Collections.Teams.Team;

   public class ListTeamRolesQueryHandler : IQueryHandler<ListTeamRolesQuery, ListTeamRolesProjection>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Team> _teamsCollection;

      public ListTeamRolesQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Team> teamsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _teamsCollection = teamsCollection;
      }

      public async Task<ListTeamRolesProjection> HandleAsync(ListTeamRolesQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
               query.TeamId
            );
         }

         var teamRoles = await _teamsCollection.Find(t => t.Id == query.TeamId)
            .Project(t => t.Roles)
            .SingleAsync(cancellationToken);

         return new ListTeamRolesProjection
         (
            teamRoles.Select(r => new Role
               (
                  r.Id.ToString(),
                  r.Name,
                  r.Permissions.Select(p => p.ToQueryPermission()).Where(p => p != null).ToList()
               ))
               .ToList()
         );
      }
   }
}