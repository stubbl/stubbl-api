namespace Stubbl.Api.Core.EventHandlers.Cache.FindAuthenticatedMember
{
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Common.Caching;
   using Common.EventHandlers;
   using Events.AuthenticatedMemberUpdated.Version1;

   public class AuthenticatedMemberUpdatedEventHandler : IEventHandler<AuthenticatedMemberUpdatedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public AuthenticatedMemberUpdatedEventHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         ICache cache, ICacheKey cacheKey)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(AuthenticatedMemberUpdatedEvent @event, CancellationToken cancellationToken)
      {
         _cache.Remove(_cacheKey.FindAuthenticatedMember
         (
            _authenticatedMemberAccessor.AuthenticatedMember.IdentityId
         ));

         return Task.CompletedTask;
      }
   }
}
