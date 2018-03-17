﻿using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Authentication;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.EventHandlers;
using Stubbl.Api.Authentication;
using Stubbl.Api.Caching;
using Stubbl.Api.Events.AuthenticatedUserInvitationAccepted.Version1;

namespace Stubbl.Api.EventHandlers.Cache.FindAuthenticatedUser
{
    public class
        AuthenticatedUserInvitationAcceptedEventHandler : IEventHandler<AuthenticatedUserInvitationAcceptedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;

        public AuthenticatedUserInvitationAcceptedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            ICache cache, ICacheKey cacheKey)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _cache = cache;
            _cacheKey = cacheKey;
        }

        public Task HandleAsync(AuthenticatedUserInvitationAcceptedEvent @event, CancellationToken cancellationToken)
        {
            _cache.Remove(_cacheKey.FindAuthenticatedUser
            (
                _authenticatedUserAccessor.AuthenticatedUser.IdentityId
            ));

            return Task.CompletedTask;
        }
    }
}