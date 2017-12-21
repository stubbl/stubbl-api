﻿namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Common.Caching;
   using Common.QueryHandlers;
   using Exceptions.MemberNotInvitedToTeam.Version1;
   using MongoDB.Driver;
   using Queries.Shared.Version1;
   using Queries.FindAuthenticatedMemberInvitation.Version1;
   using Invitation = Data.Collections.Invitations.Invitation;
   using InvitationStatus = Data.Collections.Invitations.InvitationStatus;

   public class FindAuthenticatedMemberInvitationQueryHandler : IQueryHandler<FindAuthenticatedMemberInvitationQuery, FindAuthenticatedMemberInvitationProjection>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Invitation> _invitationsCollection;

      public FindAuthenticatedMemberInvitationQueryHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         ICache cache, ICacheKey cacheKey, IMongoCollection<Invitation> invitationsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
         _invitationsCollection = invitationsCollection;
      }

      public async Task<FindAuthenticatedMemberInvitationProjection> HandleAsync(FindAuthenticatedMemberInvitationQuery query, CancellationToken cancellationToken)
      {
         var invitation = await _cache.GetOrSetAsync
         (
            _cacheKey.FindAuthenticatedMemberInvitation(_authenticatedMemberAccessor.AuthenticatedMember.EmailAddress, query.InvitationId),
            async () => await _invitationsCollection.Find(i => i.Id == query.InvitationId && i.EmailAddress.ToLower() == _authenticatedMemberAccessor.AuthenticatedMember.EmailAddress.ToLower() && i.Status == InvitationStatus.Sent)
               .SingleOrDefaultAsync(cancellationToken)
         );

         if (invitation == null)
         {
            throw new MemberNotInvitedToTeamException
            (
               _authenticatedMemberAccessor.AuthenticatedMember.Id,
               query.InvitationId
            );
         }

         return new FindAuthenticatedMemberInvitationProjection
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