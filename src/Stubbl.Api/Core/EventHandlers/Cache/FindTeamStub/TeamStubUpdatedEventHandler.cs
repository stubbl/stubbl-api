namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeamStub
{
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using Common.Caching;
   using Common.EventHandlers;
   using Events.TeamStubUpdated.Version1;

   public class TeamStubUpdatedEventHandler : IEventHandler<TeamStubUpdatedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public TeamStubUpdatedEventHandler(ICache cache, ICacheKey cacheKey)
      {
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(TeamStubUpdatedEvent @event, CancellationToken cancellationToken)
      {
         _cache.Remove(_cacheKey.FindTeamStub
         (
            @event.TeamId,
            @event.StubId
         ));

         return Task.CompletedTask;
      }
   }
}
