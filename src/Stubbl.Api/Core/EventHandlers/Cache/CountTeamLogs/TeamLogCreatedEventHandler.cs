namespace Stubbl.Api.Core.EventHandlers.Cache.CountTeamLogs
{
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using Gunnsoft.Common.Caching;
   using Gunnsoft.Cqs.EventHandlers;
   using Events.TeamLogCreated.Version1;

   public class TeamLogCreatedEventHandler : IEventHandler<TeamLogCreatedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public TeamLogCreatedEventHandler(ICache cache, ICacheKey cacheKey)
      {
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(TeamLogCreatedEvent @event, CancellationToken cancellationToken)
      {
         _cache.Remove(_cacheKey.CountTeamLogs
         (
            @event.TeamId
         ));

         return Task.CompletedTask;
      }
   }
}