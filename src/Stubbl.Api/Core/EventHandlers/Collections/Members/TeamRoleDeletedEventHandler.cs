namespace Stubbl.Api.Core.EventHandlers.Collections.Members
{
   using System.Linq;
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using CodeContrib.EventHandlers;
   using Data;
   using Data.Collections.Members;
   using Events.TeamRoleDeleted.Version1;
   using MongoDB.Driver;
   using Team = Data.Collections.Teams.Team;

   public class TeamRoleDeletedEventHandler : IEventHandler<TeamRoleDeletedEvent>
   {
      private readonly IMongoCollection<Member> _membersCollection;
      private readonly IMongoCollection<Team> _teamsCollection;

      public TeamRoleDeletedEventHandler(IMongoCollection<Member> membersCollection,
         IMongoCollection<Team> teamsCollection)
      {
         _membersCollection = membersCollection;
         _teamsCollection = teamsCollection;
      }

      public async Task HandleAsync(TeamRoleDeletedEvent @event, CancellationToken cancellationToken)
      {
         var members = await _membersCollection.Find(t => t.Teams.Any(r => r.Id == @event.TeamId))
            .ToListAsync(cancellationToken);

         var requests = new List<WriteModel<Member>>();

         foreach (var member in members)
         {
            var userRole = await _teamsCollection.Find(t => t.Id == @event.TeamId)
               .Project(t => t.Roles.Single(r => r.Name.ToLower() == DefaultRoleNames.User.ToLower() && r.IsDefault))
               .SingleAsync(cancellationToken);

            var team = member.Teams.Single(t => t.Id == @event.TeamId);
            team.Role.Name = userRole.Name;
            team.Role.Permissions = userRole.Permissions;

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
