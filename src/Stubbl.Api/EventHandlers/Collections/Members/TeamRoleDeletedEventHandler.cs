using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Data;
using Stubbl.Api.Data.Collections.Users;
using Stubbl.Api.Events.TeamRoleDeleted.Version1;
using Team = Stubbl.Api.Data.Collections.Teams.Team;

namespace Stubbl.Api.EventHandlers.Collections.Members
{
    public class TeamRoleDeletedEventHandler : IEventHandler<TeamRoleDeletedEvent>
    {
        private readonly IMongoCollection<Team> _teamsCollection;
        private readonly IMongoCollection<User> _usersCollection;

        public TeamRoleDeletedEventHandler(IMongoCollection<Team> teamsCollection,
            IMongoCollection<User> usersCollection)
        {
            _teamsCollection = teamsCollection;
            _usersCollection = usersCollection;
        }

        public async Task HandleAsync(TeamRoleDeletedEvent @event, CancellationToken cancellationToken)
        {
            var users = await _usersCollection.Find(t => t.Teams.Any(r => r.Id == @event.TeamId))
                .ToListAsync(cancellationToken);

            var requests = new List<WriteModel<User>>();

            foreach (var user in users)
            {
                var userRole = await _teamsCollection.Find(t => t.Id == @event.TeamId)
                    .Project(t =>
                        t.Roles.Single(r => r.Name.ToLower() == DefaultRoleNames.User.ToLower() && r.IsDefault))
                    .SingleAsync(cancellationToken);

                var team = user.Teams.Single(t => t.Id == @event.TeamId);
                team.Role.Name = userRole.Name;
                team.Role.Permissions = userRole.Permissions;

                var filter = Builders<User>.Filter.Where(m => m.Id == user.Id);
                var pullUpdate = Builders<User>.Update.PullFilter(m => m.Teams, t => t.Id == @event.TeamId);
                var pushUpdate = Builders<User>.Update.Push(m => m.Teams, team);

                requests.Add(new UpdateOneModel<User>(filter, pullUpdate));
                requests.Add(new UpdateOneModel<User>(filter, pushUpdate));
            }

            await _usersCollection.BulkWriteAsync(requests, cancellationToken: cancellationToken);
        }
    }
}