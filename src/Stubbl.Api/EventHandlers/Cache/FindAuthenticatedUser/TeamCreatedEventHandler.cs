﻿using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.Events;
using Stubbl.Api.Authentication;
using Stubbl.Api.Caching;
using Stubbl.Api.Events.TeamCreated.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindAuthenticatedUser
{
    public class TeamCreatedEventHandler : IEventHandler<TeamCreatedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;

        public TeamCreatedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            ICache cache, ICacheKey cacheKey)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public Task HandleAsync(TeamCreatedEvent @event, CancellationToken cancellationToken)
        {
            _cache.Remove(_cacheKey.FindAuthenticatedUser
            (
                _authenticatedUserAccessor.AuthenticatedUser.Sub
            ));

            return Task.CompletedTask;
        }
    }
}