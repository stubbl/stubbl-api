namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeamInvitation
{
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using Common.Caching;
   using Common.EventHandlers;
   using Events.TeamInvitationDeleted.Version1;

   public class TeamInvitationDeletedEventHandler : IEventHandler<TeamInvitationDeletedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public TeamInvitationDeletedEventHandler(ICache cache, ICacheKey cacheKey)
      {
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(TeamInvitationDeletedEvent @event, CancellationToken cancellationToken)
      {
         _cache.Remove(_cacheKey.FindTeamInvitation
         (
            @event.TeamId,
            @event.InvitationId
         ));

         return Task.FromResult(0);
      }
   }
}
