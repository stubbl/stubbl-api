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
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Invitation> _invitationsCollection;

      public CountTeamInvitationsQueryHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         ICache cache, ICacheKey cacheKey, IMongoCollection<Invitation> invitationsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
         _invitationsCollection = invitationsCollection;
      }

      public async Task<CountTeamInvitationsProjection> HandleAsync(CountTeamInvitationsQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedMemberAccessor.AuthenticatedMember.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedMemberAccessor.AuthenticatedMember.Id,
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
