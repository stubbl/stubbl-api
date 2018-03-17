namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeamInvitation
{
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using Gunnsoft.Common.Caching;
   using Gunnsoft.Cqs.EventHandlers;
   using Data.Collections.Invitations;
   using Events.TeamUpdated.Version1;
   using MongoDB.Driver;

   public class TeamUpdatedEventHandler : IEventHandler<TeamUpdatedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Invitation> _invitationsCollection;

      public TeamUpdatedEventHandler(ICache cache, ICacheKey cacheKey, IMongoCollection<Invitation> invitationsCollection)
      {
         _cache = cache;
         _cacheKey = cacheKey;
         _invitationsCollection = invitationsCollection;
      }

      public async Task HandleAsync(TeamUpdatedEvent @event, CancellationToken cancellationToken)
      {
         var invitationIds = await _invitationsCollection.Find(i => i.Team.Id == @event.TeamId)
            .Project(i => i.Id)
            .ToListAsync(cancellationToken);

         foreach (var invitationId in invitationIds)
         {
            _cache.Remove(_cacheKey.FindTeamInvitation
            (
               @event.TeamId,
               invitationId
            ));
         }
      }
   }
}
