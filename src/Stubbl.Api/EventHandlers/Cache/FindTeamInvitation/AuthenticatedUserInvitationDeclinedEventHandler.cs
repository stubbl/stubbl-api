﻿using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.EventHandlers;
using MongoDB.Driver;
using Stubbl.Api.Caching;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Events.AuthenticatedUserInvitationDeclined.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindTeamInvitation
{
    public class
        AuthenticatedUserInvitationDeclinedEventHandler : IEventHandler<AuthenticatedUserInvitationDeclinedEvent>
    {
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;
        private readonly IMongoCollection<Invitation> _invitationsCollection;

        public AuthenticatedUserInvitationDeclinedEventHandler(ICache cache, ICacheKey cacheKey,
            IMongoCollection<Invitation> invitationsCollection)
        {
            _cache = cache;
            _cacheKey = cacheKey;
            _invitationsCollection = invitationsCollection;
        }

        public async Task HandleAsync(AuthenticatedUserInvitationDeclinedEvent @event,
            CancellationToken cancellationToken)
        {
            var teamId = await _invitationsCollection.Find(i => i.Id == @event.InvitationId)
                .Project(i => i.Team.Id)
                .SingleAsync(cancellationToken);

            _cache.Remove(_cacheKey.FindTeamInvitation
            (
                teamId,
                @event.InvitationId
            ));
        }
    }
}