namespace Stubbl.Api.Core.EventHandlers.Cache.FindAuthenticatedUser
{
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using CodeContrib.Caching;
   using CodeContrib.EventHandlers;
   using Data.Collections.Members;
   using Events.TeamMemberRemoved.Version1;
   using MongoDB.Driver;

   public class TeamMemberRemovedEventHandler : IEventHandler<TeamMemberRemovedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Member> _membersCollection;

      public TeamMemberRemovedEventHandler(ICache cache, ICacheKey cacheKey, IMongoCollection<Member> membersCollection)
      {
         _cache = cache;
         _cacheKey = cacheKey;
         _membersCollection = membersCollection;
      }

      public async Task HandleAsync(TeamMemberRemovedEvent @event, CancellationToken cancellationToken)
      {
         var identityId = await _membersCollection.Find(m => m.Id == @event.MemberId)
            .Project(m => m.IdentityId)
            .SingleAsync(cancellationToken);

         _cache.Remove(_cacheKey.FindAuthenticatedUser
         (
            identityId
         ));
      }
   }
}
