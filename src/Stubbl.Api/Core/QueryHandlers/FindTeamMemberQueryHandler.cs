namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Gunnsoft.Common.Caching;
   using Gunnsoft.Cqs.QueryHandlers;
   using Data.Collections.Members;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using Exceptions.MemberNotFound.Version1;
   using MongoDB.Driver;
   using Queries.FindTeamMember.Version1;
   using Queries.Shared.Version1;
   using Role = Queries.FindTeamMember.Version1.Role;

   public class FindTeamMemberQueryHandler : IQueryHandler<FindTeamMemberQuery, FindTeamMemberProjection>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Member> _membersCollection;

      public FindTeamMemberQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         ICache cache, ICacheKey cacheKey, IMongoCollection<Member> membersCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
         _membersCollection = membersCollection;
      }

      public async Task<FindTeamMemberProjection> HandleAsync(FindTeamMemberQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
               query.TeamId
            );
         }

         var member = await _cache.GetOrSetAsync
         (
            _cacheKey.FindTeamMember(query.TeamId, query.MemberId), 
            async () => await _membersCollection.Find(m => m.Teams.Any(t => t.Id == query.TeamId) && m.Id == query.MemberId)
               .Project(m => new FindTeamMemberProjection
               (
                  m.Id.ToString(),
                  m.Name,
                  m.EmailAddress,
                  new Role
                  (
                     m.Teams.Single(t => t.Id == query.TeamId).Role.Id.ToString(),
                     m.Teams.Single(t => t.Id == query.TeamId).Role.Name,
                     m.Teams.Single(t => t.Id == query.TeamId).Role.Permissions.Select(p => p.ToQueryPermission()).Where(p => p != null).ToList()
                  )
               ))
               .SingleOrDefaultAsync(cancellationToken)
         );

         if (member == null)
         {
            throw new MemberNotFoundException
            (
               query.MemberId,
               query.TeamId
            );
         }

         return member;
      }
   }
}