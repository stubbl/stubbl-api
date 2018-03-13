namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeamMember
{
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Common.Caching;
   using Common.EventHandlers;
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
         _cache.Remove(_cacheKey.FindTeamMember
         (
            @event.TeamId,
            _authenticatedUserAccessor.AuthenticatedUser.Id
         ));

         return Task.CompletedTask;
      }
   }
}
