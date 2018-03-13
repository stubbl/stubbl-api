namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using Common.Caching;
   using Common.QueryHandlers;
   using Core.Authentication;
   using Core.Exceptions.MemberNotAddedToTeam.Version1;
   using Data.Collections.Invitations;
   using MongoDB.Driver;
   using Queries.CountTeamInvitations.Version1;

   public class CountTeamInvitationsQueryHandler : IQueryHandler<CountTeamInvitationsQuery, CountTeamInvitationsProjection>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Invitation> _invitationsCollection;

      public CountTeamInvitationsQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         ICache cache, ICacheKey cacheKey, IMongoCollection<Invitation> invitationsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
         _invitationsCollection = invitationsCollection;
      }

      public async Task<CountTeamInvitationsProjection> HandleAsync(CountTeamInvitationsQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
               query.TeamId
            );
         }

         var totalCount = await _cache.GetOrSetAsync
         (
            _cacheKey.CountTeamInvitations(query.TeamId),
            async () => await _invitationsCollection.CountAsync(i => i.Team.Id == query.TeamId)
         );

         return new CountTeamInvitationsProjection
         (
            totalCount
         );
      }
   }
}
