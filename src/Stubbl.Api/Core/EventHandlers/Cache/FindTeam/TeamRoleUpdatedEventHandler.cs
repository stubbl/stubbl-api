namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeam
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using CodeContrib.Caching;
   using CodeContrib.EventHandlers;
   using Data.Collections.Teams;
   using Events.TeamRoleUpdated.Version1;
   using MongoDB.Driver;

   public class TeamRoleUpdatedEventHandler : IEventHandler<TeamRoleUpdatedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Team> _teamsCollection;

      public TeamRoleUpdatedEventHandler(ICache cache, ICacheKey cacheKey, IMongoCollection<Team> teamsCollection)
      {
         _cache = cache;
         _cacheKey = cacheKey;
         _teamsCollection = teamsCollection;
      }

      public async Task HandleAsync(TeamRoleUpdatedEvent @event, CancellationToken cancellationToken)
      {
         var teamIds = await _teamsCollection.Find(t => t.Roles.Any(r => r.Id == @event.RoleId))
            .Project(t => t.Id)
            .ToListAsync(cancellationToken);

         foreach (var teamId in teamIds)
         {
            _cache.Remove(_cacheKey.FindTeam
            (
               teamId
            ));
         }
      }
   }
}
