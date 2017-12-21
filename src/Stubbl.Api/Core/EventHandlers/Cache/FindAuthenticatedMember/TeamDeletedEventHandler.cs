namespace Stubbl.Api.Core.EventHandlers.Cache.FindAuthenticatedMember
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Common.Caching;
   using Common.EventHandlers;
   using Data.Collections.Members;
   using Events.TeamDeleted.Version1;
   using MongoDB.Driver;

   public class TeamDeletedEventHandler : IEventHandler<TeamDeletedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Member> _membersCollection;

      public TeamDeletedEventHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         ICache cache, ICacheKey cacheKey, IMongoCollection<Member> membersCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
         _membersCollection = membersCollection;
      }

      public async Task HandleAsync(TeamDeletedEvent @event, CancellationToken cancellationToken)
      {
         var identityIds = await _membersCollection.Find(m => m.Teams.Any(t => t.Id == @event.TeamId))
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
