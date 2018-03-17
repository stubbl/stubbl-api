namespace Stubbl.Api.Core.EventHandlers.Cache.FindAuthenticatedUser
{
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Gunnsoft.Common.Caching;
   using Gunnsoft.Cqs.EventHandlers;
   using Events.AuthenticatedUserUpdated.Version1;

   public class AuthenticatedUserUpdatedEventHandler : IEventHandler<AuthenticatedUserUpdatedEvent>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public AuthenticatedUserUpdatedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         ICache cache, ICacheKey cacheKey)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(AuthenticatedUserUpdatedEvent @event, CancellationToken cancellationToken)
      {
         _cache.Remove(_cacheKey.FindAuthenticatedUser
         (
            _authenticatedUserAccessor.AuthenticatedUser.IdentityId
         ));

         return Task.CompletedTask;
      }
   }
}
