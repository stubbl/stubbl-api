namespace Stubbl.Api.Core.EventHandlers.Cache.CountTeamInvitations
{
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using CodeContrib.Caching;
   using CodeContrib.EventHandlers;
   using Events.TeamInvitationCreated.Version1;

   public class TeamInvitationCreatedEventHandler : IEventHandler<TeamInvitationCreatedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public TeamInvitationCreatedEventHandler(ICache cache, ICacheKey cacheKey)
      {
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(TeamInvitationCreatedEvent @event, CancellationToken cancellationToken)
      {
         _cache.Remove(_cacheKey.CountTeamInvitations
         (
            @event.TeamId
         ));

         return Task.CompletedTask;
      }
   }
}