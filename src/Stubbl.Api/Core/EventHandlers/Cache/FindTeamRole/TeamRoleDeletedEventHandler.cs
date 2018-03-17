namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeamRole
{
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using Gunnsoft.Common.Caching;
   using Gunnsoft.Cqs.EventHandlers;
   using Events.TeamRoleDeleted.Version1;

   public class TeamRoleDeletedEventHandler : IEventHandler<TeamRoleDeletedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public TeamRoleDeletedEventHandler(ICache cache, ICacheKey cacheKey)
      {
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(TeamRoleDeletedEvent @event, CancellationToken cancellationToken)
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
