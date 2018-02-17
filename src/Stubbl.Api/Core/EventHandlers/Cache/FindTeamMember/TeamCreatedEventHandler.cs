namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeamMember
{
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Common.Caching;
   using Common.EventHandlers;
   using Events.TeamCreated.Version1;

   public class TeamCreatedEventHandler : IEventHandler<TeamCreatedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public TeamCreatedEventHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         ICache cache, ICacheKey cacheKey)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(TeamCreatedEvent @event, CancellationToken cancellationToken)
      {
         _cache.Remove(_cacheKey.FindTeamMember
         (
            @event.TeamId,
            _authenticatedMemberAccessor.AuthenticatedMember.Id
         ));

         return Task.CompletedTask;
      }
   }
}
