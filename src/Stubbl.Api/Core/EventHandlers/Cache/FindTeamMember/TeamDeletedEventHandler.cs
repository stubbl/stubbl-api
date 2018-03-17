namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeamMember
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using Gunnsoft.Common.Caching;
   using Gunnsoft.Cqs.EventHandlers;
   using Data.Collections.Members;
   using Events.TeamDeleted.Version1;
   using MongoDB.Driver;

   public class TeamDeletedEventHandler : IEventHandler<TeamDeletedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Member> _membersCollection;

      public TeamDeletedEventHandler(ICache cache, ICacheKey cacheKey, IMongoCollection<Member> membersCollection)
      {
         _cache = cache;
         _cacheKey = cacheKey;
         _membersCollection = membersCollection;
      }

      public async Task HandleAsync(TeamDeletedEvent @event, CancellationToken cancellationToken)
      {
         var memberIds = await _membersCollection.Find(m => m.Teams.Any(t => t.Id == @event.TeamId))
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
