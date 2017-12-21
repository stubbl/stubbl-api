namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeamMember
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
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Member> _membersCollection;

      public TeamUpdatedEventHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         ICache cache, ICacheKey cacheKey, IMongoCollection<Member> membersCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
         _membersCollection = membersCollection;
      }

      public async Task HandleAsync(TeamUpdatedEvent @event, CancellationToken cancellationToken)
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