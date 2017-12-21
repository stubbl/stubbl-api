namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeamMember
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
         var memberIds = await _membersCollection.Find(t => t.Teams.Any(r => r.Id == @event.TeamId))
            .Project(m => m.Id)
            .ToListAsync(cancellationToken);

         foreach (var memberId in memberIds)
         {
            _cache.Remove(_cacheKey.FindTeamMember
            (
               @event.TeamId,
               memberId
            ));
         }
      }
   }
}
