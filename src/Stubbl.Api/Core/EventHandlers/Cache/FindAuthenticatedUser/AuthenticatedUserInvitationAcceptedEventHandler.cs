namespace Stubbl.Api.Core.EventHandlers.Cache.FindAuthenticatedUser
{
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Common.Caching;
   using Common.EventHandlers;
   using Events.AuthenticatedUserInvitationAccepted.Version1;

   public class AuthenticatedUserInvitationAcceptedEventHandler : IEventHandler<AuthenticatedUserInvitationAcceptedEvent>
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
