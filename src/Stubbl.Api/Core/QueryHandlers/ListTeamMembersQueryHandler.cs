namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using CodeContrib.QueryHandlers;
   using Data.Collections.Teams;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using MongoDB.Driver;
   using Queries.ListTeamMembers.Version1;
   using Queries.Shared.Version1;
   using Member = Queries.ListTeamMembers.Version1.Member;
   using Role = Queries.ListTeamMembers.Version1.Role;

   public class ListTeamMembersQueryHandler : IQueryHandler<ListTeamMembersQuery, ListTeamMembersProjection>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Team> _teamsCollection;

      public ListTeamMembersQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Team> teamsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _teamsCollection = teamsCollection;
      }

      public async Task<ListTeamMembersProjection> HandleAsync(ListTeamMembersQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
               query.TeamId
            );
         }

         var filter = Builders<Team>.Filter.Where(t => t.Id == query.TeamId);
         var totalCount = await _teamsCollection.Find(filter)
            .Project(t => t.Members.Count)
            .SingleOrDefaultAsync(cancellationToken);
         var members = await _teamsCollection.Find(filter)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Limit(query.PageSize)
            .Project(t => t.Members.Select(m => new Member
               (
                  m.Id.ToString(),
                  m.Name,
                  m.EmailAddress,
                  new Role
                  (
                     m.Role.Id.ToString(),
                     t.Roles.Single(r => r.Id == m.Role.Id).Name
                  )  
               ))
               .ToList())
            .SingleOrDefaultAsync(cancellationToken);

         return new ListTeamMembersProjection
         (
            members,
            new Paging
            (
               query.PageNumber,
               query.PageSize,
               totalCount
            )
         );
      }
   }
}