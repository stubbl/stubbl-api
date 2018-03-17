namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeam
{
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using Gunnsoft.Common.Caching;
   using Gunnsoft.Cqs.EventHandlers;
   using Events.TeamUpdated.Version1;

   public class TeamUpdatedEventHandler : IEventHandler<TeamUpdatedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public TeamUpdatedEventHandler(ICache cache, ICacheKey cacheKey)
      {
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(TeamUpdatedEvent @event, CancellationToken cancellationToken)
      {
         _cache.Remove(_cacheKey.FindTeam
         (
            @event.TeamId
         ));

         return Task.CompletedTask;
      }
   }
}
