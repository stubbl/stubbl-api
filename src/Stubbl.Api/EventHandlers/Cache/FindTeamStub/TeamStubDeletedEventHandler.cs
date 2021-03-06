﻿using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.Events;
using Stubbl.Api.Caching;
using Stubbl.Api.Events.TeamStubDeleted.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindTeamStub
{
    public class TeamStubDeletedEventHandler : IEventHandler<TeamStubDeletedEvent>
    {
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;

        public TeamStubDeletedEventHandler(ICache cache, ICacheKey cacheKey)
        {
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public Task HandleAsync(TeamStubDeletedEvent @event, CancellationToken cancellationToken)
        {
            _cache.Remove(_cacheKey.FindTeamStub
            (
                @event.TeamId,
                @event.StubId
            ));

            return Task.CompletedTask;
        }
    }
}