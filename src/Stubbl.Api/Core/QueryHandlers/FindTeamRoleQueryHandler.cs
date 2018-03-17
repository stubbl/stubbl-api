namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using MongoDB.Driver;
   using Authentication;
   using Caching;
   using Gunnsoft.Common.Caching;
   using Gunnsoft.Cqs.QueryHandlers;
   using Data.Collections.Teams;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using Exceptions.RoleNotFound.Version1;
   using Queries.FindTeamRole.Version1;
   using Queries.Shared.Version1;

   public class FindTeamRoleQueryHandler : IQueryHandler<FindTeamRoleQuery, FindTeamRoleProjection>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Team> _teamsCollection;

      public FindTeamRoleQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         ICache cache, ICacheKey cacheKey, IMongoCollection<Team> teamsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
         _teamsCollection = teamsCollection;
      }

      public async Task<FindTeamRoleProjection> HandleAsync(FindTeamRoleQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
               query.TeamId
            );
         }

         var role = await _cache.GetOrSetAsync
         (
            _cacheKey.FindTeamRole(query.TeamId, query.RoleId),
            async () => await _teamsCollection.Find(t => t.Id == query.TeamId && t.Roles.Any(r => r.Id == query.RoleId))
               .Project(t => t.Roles.SingleOrDefault(r => r.Id == query.RoleId))
               .SingleOrDefaultAsync(cancellationToken)
         );

         if (role == null)
         {
            throw new RoleNotFoundException
            (
               query.RoleId,
               query.TeamId
            );
         }

         return new FindTeamRoleProjection
         (
            role.Id.ToString(),
            role.Name,
            role.Permissions.Select(p => p.ToQueryPermission()).Where(p => p != null).ToList()
         );
      }
   }
}