namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeam
{
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using CodeContrib.Caching;
   using CodeContrib.EventHandlers;
   using Events.TeamMemberRemoved.Version1;

   public class TeamMemberRemovedEventHandler : IEventHandler<TeamMemberRemovedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public TeamMemberRemovedEventHandler(ICache cache, ICacheKey cacheKey)
      {
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(TeamMemberRemovedEvent @event, CancellationToken cancellationToken)
      {
         _cache.Remove(_cacheKey.FindTeam
         (
            @event.TeamId
         ));

         return Task.CompletedTask;
      }
   }
}
