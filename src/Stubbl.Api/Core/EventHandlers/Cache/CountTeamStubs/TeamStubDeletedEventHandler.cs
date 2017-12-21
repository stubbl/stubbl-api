namespace Stubbl.Api.Core.EventHandlers.Cache.CountTeamStubs
{
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using Common.Caching;
   using Common.EventHandlers;
   using Events.TeamStubDeleted.Version1;

   public class TeamStubDeletedEventHandler : IEventHandler<TeamStubDeletedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public TeamStubDeletedEventHandler(ICache cache, ICacheKey cacheKey)
      {
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(TeamStubDeletedEvent @event, CancellationToken cancellationToken)
      {
         _cache.Remove(_cacheKey.CountTeamStubs
         (
            @event.TeamId
         ));

         return Task.FromResult(0);
      }
   }
}