namespace Stubbl.Api.Core.EventHandlers.Cache.FindAuthenticatedUser
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Common.Caching;
   using Common.EventHandlers;
   using Data.Collections.Members;
   using Events.TeamUpdated.Version1;
   using MongoDB.Driver;

   public class TeamUpdatedEventHandler : IEventHandler<TeamUpdatedEvent>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Member> _membersCollection;

      public TeamUpdatedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         ICache cache, ICacheKey cacheKey, IMongoCollection<Member> membersCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
         _membersCollection = membersCollection;
      }

      public async Task HandleAsync(TeamUpdatedEvent @event, CancellationToken cancellationToken)
      {
         var identityIds = await _membersCollection.Find(m => m.Teams.Any(t => t.Id == @event.TeamId))
            .Project(m => m.IdentityId)
            .ToListAsync(cancellationToken);

         foreach (var identityId in identityIds)
         {
            _cache.Remove(_cacheKey.FindAuthenticatedUser
            (
               identityId
            ));
         }
      }
   }

}
