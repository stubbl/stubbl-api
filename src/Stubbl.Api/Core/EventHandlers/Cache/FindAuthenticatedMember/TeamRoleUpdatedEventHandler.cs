namespace Stubbl.Api.Core.EventHandlers.Cache.FindAuthenticatedMember
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using Common.Caching;
   using Common.EventHandlers;
   using Data.Collections.Members;
   using Events.TeamRoleUpdated.Version1;
   using MongoDB.Driver;

   public class TeamRoleUpdatedEventHandler : IEventHandler<TeamRoleUpdatedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Member> _membersCollection;

      public TeamRoleUpdatedEventHandler(ICache cache, ICacheKey cacheKey, IMongoCollection<Member> membersCollection)
      {
         _cache = cache;
         _cacheKey = cacheKey;
         _membersCollection = membersCollection;
      }

      public async Task HandleAsync(TeamRoleUpdatedEvent @event, CancellationToken cancellationToken)
      {
         var identityIds = await _membersCollection.Find(t => t.Teams.Any(r => r.Id == @event.TeamId))
            .Project(m => m.IdentityId)
            .ToListAsync(cancellationToken);

         foreach (var identityId in identityIds)
         {
            _cache.Remove(_cacheKey.FindAuthenticatedMember
            (
               identityId
            ));
         }
      }
   }
}
