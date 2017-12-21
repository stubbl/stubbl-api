namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeam
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Common.Caching;
   using Common.EventHandlers;
   using Events.AuthenticatedMemberUpdated.Version1;

   public class AuthenticatedMemberUpdatedEventHandler : IEventHandler<AuthenticatedMemberUpdatedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public AuthenticatedMemberUpdatedEventHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         ICache cache, ICacheKey cacheKey)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(AuthenticatedMemberUpdatedEvent @event, CancellationToken cancellationToken)
      {
         foreach (var teamId in _authenticatedMemberAccessor.AuthenticatedMember.Teams.Select(t => t.Id))
         {
            _cache.Remove(_cacheKey.FindTeamMember
            (
               teamId,
               _authenticatedMemberAccessor.AuthenticatedMember.Id
            ));
         }
         return Task.FromResult(0);
      }
   }
}
