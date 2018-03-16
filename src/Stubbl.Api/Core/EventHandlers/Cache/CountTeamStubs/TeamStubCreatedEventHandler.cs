namespace Stubbl.Api.Core.EventHandlers.Cache.CountTeamStubs
{
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using CodeContrib.Caching;
   using CodeContrib.EventHandlers;
   using Events.TeamStubCreated.Version1;

   public class TeamStubCreatedEventHandler : IEventHandler<TeamStubCreatedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public TeamStubCreatedEventHandler(ICache cache, ICacheKey cacheKey)
      {
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(TeamStubCreatedEvent @event, CancellationToken cancellationToken)
      {
         _cache.Remove(_cacheKey.CountTeamStubs
         (
            @event.TeamId
         ));

         return Task.CompletedTask;
      }
   }
}