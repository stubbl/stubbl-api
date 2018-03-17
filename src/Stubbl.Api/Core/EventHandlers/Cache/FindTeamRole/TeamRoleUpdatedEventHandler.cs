namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeamRole
{
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using Gunnsoft.Common.Caching;
   using Gunnsoft.Cqs.EventHandlers;
   using Events.TeamRoleUpdated.Version1;

   public class TeamRoleUpdatedEventHandler : IEventHandler<TeamRoleUpdatedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public TeamRoleUpdatedEventHandler(ICache cache, ICacheKey cacheKey)
      {
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(TeamRoleUpdatedEvent @event, CancellationToken cancellationToken)
      {
         _cache.Remove(_cacheKey.FindTeamRole
         (
            @event.TeamId,
            @event.RoleId
         ));

         return Task.CompletedTask;
      }
   }
}
