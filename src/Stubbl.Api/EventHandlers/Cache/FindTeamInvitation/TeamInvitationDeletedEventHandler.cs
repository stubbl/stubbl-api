﻿using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.Events;
using Stubbl.Api.Caching;
using Stubbl.Api.Events.TeamInvitationDeleted.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindTeamInvitation
{
    public class TeamInvitationDeletedEventHandler : IEventHandler<TeamInvitationDeletedEvent>
    {
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;

        public TeamInvitationDeletedEventHandler(ICache cache, ICacheKey cacheKey)
        {
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public Task HandleAsync(TeamInvitationDeletedEvent @event, CancellationToken cancellationToken)
        {
            _cache.Remove(_cacheKey.FindTeamInvitation
            (
                @event.TeamId,
                @event.InvitationId
            ));

            return Task.CompletedTask;
        }
    }
}