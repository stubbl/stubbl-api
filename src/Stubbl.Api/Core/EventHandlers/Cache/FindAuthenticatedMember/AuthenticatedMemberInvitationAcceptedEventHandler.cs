﻿namespace Stubbl.Api.Core.EventHandlers.Cache.FindAuthenticatedMember
{
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Common.Caching;
   using Common.EventHandlers;
   using Events.AuthenticatedMemberInvitationAccepted.Version1;

   public class AuthenticatedMemberInvitationAcceptedEventHandler : IEventHandler<AuthenticatedMemberInvitationAcceptedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public AuthenticatedMemberInvitationAcceptedEventHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         ICache cache, ICacheKey cacheKey)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(AuthenticatedMemberInvitationAcceptedEvent @event, CancellationToken cancellationToken)
      {
         _cache.Remove(_cacheKey.FindAuthenticatedMember
         (
            _authenticatedMemberAccessor.AuthenticatedMember.IdentityId
         ));

         return Task.FromResult(0);
      }
   }
}
