namespace Stubbl.Api.Core.EventHandlers.Cache.CountTeamLogs
{
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using CodeContrib.Caching;
   using CodeContrib.EventHandlers;
   using Events.TeamDeleted.Version1;

   public class TeamDeletedEventHandler : IEventHandler<TeamDeletedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public TeamDeletedEventHandler(ICache cache, ICacheKey cacheKey)
      {
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(TeamDeletedEvent @event, CancellationToken cancellationToken)
      {
         _cache.Remove(_cacheKey.CountTeamLogs
         (
            @event.TeamId
         ));

         return Task.CompletedTask;
      }
   }
}