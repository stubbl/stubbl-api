namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeamMember
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using CodeContrib.Caching;
   using CodeContrib.EventHandlers;
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
         foreach (var teamId in _authenticatedUserAccessor.AuthenticatedUser.Teams.Select(t => t.Id))
         {
            _cache.Remove(_cacheKey.FindTeamMember
            (
               teamId,
               _authenticatedUserAccessor.AuthenticatedUser.Id
            ));
         }
         return Task.CompletedTask;
      }
   }
}
