using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Data.Collections.Users;
using Stubbl.Api.Events.Shared.Version1;
using Stubbl.Api.Events.TeamRoleUpdated.Version1;

namespace Stubbl.Api.EventHandlers.Collections.Members
{
    public class TeamRoleUpdatedEventHandler : IEventHandler<TeamRoleUpdatedEvent>
    {
        private readonly IMongoCollection<User> _usersCollection;

        public TeamRoleUpdatedEventHandler(IMongoCollection<User> usersCollection)
        {
            _usersCollection = usersCollection;
        }

        public async Task HandleAsync(TeamRoleUpdatedEvent @event, CancellationToken cancellationToken)
        {
            var members = await _usersCollection.Find(t => t.Teams.Any(r => r.Id == @event.TeamId))
                .ToListAsync(cancellationToken);

            var requests = new List<WriteModel<User>>();

            foreach (var member in members)
            {
                var team = member.Teams.Single(t => t.Id == @event.TeamId);
                team.Role.Name = @event.Name;
                team.Role.Permissions = @event.Permissions.ToDataPermissions();

                var filter = Builders<User>.Filter.Where(m => m.Id == member.Id);
                var pullUpdate = Builders<User>.Update.PullFilter(m => m.Teams, t => t.Id == @event.TeamId);
                var pushUpdate = Builders<User>.Update.Push(m => m.Teams, team);

                requests.Add(new UpdateOneModel<User>(filter, pullUpdate));
                requests.Add(new UpdateOneModel<User>(filter, pushUpdate));
            }

            await _usersCollection.BulkWriteAsync(requests, cancellationToken: cancellationToken);
        }
    }
}