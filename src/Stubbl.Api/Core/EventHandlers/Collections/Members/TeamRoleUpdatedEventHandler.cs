namespace Stubbl.Api.Core.EventHandlers.Collections.Members
{
   using System.Linq;
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using CodeContrib.EventHandlers;
   using Data.Collections.Members;
   using Events.TeamRoleUpdated.Version1;
   using Events.Shared.Version1;
   using MongoDB.Driver;

   public class TeamRoleUpdatedEventHandler : IEventHandler<TeamRoleUpdatedEvent>
   {
      private readonly IMongoCollection<Member> _membersCollection;

      public TeamRoleUpdatedEventHandler(IMongoCollection<Member> membersCollection)
      {
         _membersCollection = membersCollection;
      }

      public async Task HandleAsync(TeamRoleUpdatedEvent @event, CancellationToken cancellationToken)
      {
         var members = await _membersCollection.Find(t => t.Teams.Any(r => r.Id == @event.TeamId))
            .ToListAsync(cancellationToken);

         var requests = new List<WriteModel<Member>>();

         foreach (var member in members)
         {
            var team = member.Teams.Single(t => t.Id == @event.TeamId);
            team.Role.Name = @event.Name;
            team.Role.Permissions = @event.Permissions.ToDataPermissions();

            var filter = Builders<Member>.Filter.Where(m => m.Id == member.Id);
            var pullUpdate = Builders<Member>.Update.PullFilter(m => m.Teams, t => t.Id == @event.TeamId);
            var pushUpdate = Builders<Member>.Update.Push(m => m.Teams, team);

            requests.Add(new UpdateOneModel<Member>(filter, pullUpdate));
            requests.Add(new UpdateOneModel<Member>(filter, pushUpdate));
         }

         await _membersCollection.BulkWriteAsync(requests, cancellationToken: cancellationToken);
      }
   }
}
