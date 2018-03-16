namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using CodeContrib.Caching;
   using CodeContrib.QueryHandlers;
   using Data.Collections.Invitations;
   using Exceptions.InvitationNotFound.Version1;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using MongoDB.Driver;
   using Queries.FindTeamInvitation.Version1;
   using Queries.Shared.Version1;
   using Role = Queries.FindTeamInvitation.Version1.Role;

   public class FindTeamInvitationQueryHandler : IQueryHandler<FindTeamInvitationQuery, FindTeamInvitationProjection>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Invitation> _invitationsCollection;

      public FindTeamInvitationQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         ICache cache, ICacheKey cacheKey, IMongoCollection<Invitation> invitationsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
         _invitationsCollection = invitationsCollection;
      }

      public async Task<FindTeamInvitationProjection> HandleAsync(FindTeamInvitationQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
               query.TeamId
            );
         }

         var invitation = await _cache.GetOrSetAsync
         (
            _cacheKey.FindTeamInvitation(query.TeamId, query.InvitationId), 
            async () => await _invitationsCollection.Find(i => i.Team.Id == query.TeamId && i.Id == query.InvitationId)
               .Project(i => new FindTeamInvitationProjection
               (
                  i.Id.ToString(),
                  new Role
                  (
                     i.Role.Id.ToString(),
                     i.Role.Name
                  ),
                  i.Status.ToInvitationStatus()
               ))
               .SingleOrDefaultAsync(cancellationToken)
         );

         if (invitation == null)
         {
            throw new InvitationNotFoundException
            (
               query.InvitationId,
               query.TeamId
            );
         }

         return invitation;
      }
   }
}
