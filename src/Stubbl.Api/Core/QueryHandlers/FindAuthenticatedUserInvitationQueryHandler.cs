namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Gunnsoft.Common.Caching;
   using Gunnsoft.Cqs.QueryHandlers;
   using Exceptions.MemberNotInvitedToTeam.Version1;
   using MongoDB.Driver;
   using Queries.Shared.Version1;
   using Queries.FindAuthenticatedUserInvitation.Version1;
   using Invitation = Data.Collections.Invitations.Invitation;
   using InvitationStatus = Data.Collections.Invitations.InvitationStatus;

   public class FindAuthenticatedUserInvitationQueryHandler : IQueryHandler<FindAuthenticatedUserInvitationQuery, FindAuthenticatedUserInvitationProjection>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Invitation> _invitationsCollection;

      public FindAuthenticatedUserInvitationQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         ICache cache, ICacheKey cacheKey, IMongoCollection<Invitation> invitationsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
         _invitationsCollection = invitationsCollection;
      }

      public async Task<FindAuthenticatedUserInvitationProjection> HandleAsync(FindAuthenticatedUserInvitationQuery query, CancellationToken cancellationToken)
      {
         var invitation = await _cache.GetOrSetAsync
         (
            _cacheKey.FindAuthenticatedUserInvitation(_authenticatedUserAccessor.AuthenticatedUser.EmailAddress, query.InvitationId),
            async () => await _invitationsCollection.Find(i => i.Id == query.InvitationId && i.EmailAddress.ToLower() == _authenticatedUserAccessor.AuthenticatedUser.EmailAddress.ToLower() && i.Status == InvitationStatus.Sent)
               .SingleOrDefaultAsync(cancellationToken)
         );

         if (invitation == null)
         {
            throw new MemberNotInvitedToTeamException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
               query.InvitationId
            );
         }

         return new FindAuthenticatedUserInvitationProjection
         (
            invitation.Id.ToString(),
            new Team
            (
               invitation.Team.Id.ToString(),
               invitation.Team.Name
            ),
            new Role
            (
               invitation.Role.Id.ToString(),
               invitation.Role.Name
            )
         );
      }
   }
}