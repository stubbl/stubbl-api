namespace Stubbl.Api.Core.EventHandlers.Cache.FindAuthenticatedUser
{
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using CodeContrib.Caching;
   using CodeContrib.EventHandlers;
   using Events.TeamCreated.Version1;

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
            _authenticatedUserAccessor.AuthenticatedUser.IdentityId
         ));

         return Task.CompletedTask;
      }
   }
}
