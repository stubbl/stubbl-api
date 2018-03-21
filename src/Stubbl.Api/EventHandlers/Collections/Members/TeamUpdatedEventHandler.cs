using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Data.Collections.Users;
using Stubbl.Api.Events.TeamUpdated.Version1;
using Team = Stubbl.Api.Data.Collections.Teams.Team;

namespace Stubbl.Api.EventHandlers.Collections.Members
{
    public class TeamUpdatedEventHandler : IEventHandler<TeamUpdatedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Team> _teamsCollection;
        private readonly IMongoCollection<User> _usersCollection;

        public TeamUpdatedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Team> teamsCollection, IMongoCollection<User> usersCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _teamsCollection = teamsCollection;
            _usersCollection = usersCollection;
        }

        public async Task HandleAsync(TeamUpdatedEvent @event, CancellationToken cancellationToken)
        {
            var memberIds = await _teamsCollection.Find(t => t.Id == @event.TeamId)
                .Project(t => t.Members.Select(m => m.Id).ToList())
                .SingleOrDefaultAsync(cancellationToken);

            var team = _authenticatedUserAccessor.AuthenticatedUser.Teams
                .Single(t => t.Id == @event.TeamId);

            team.Name = @event.Name;

            var filter = Builders<User>.Filter.Where(m => memberIds.Contains(m.Id));
            var pullUpdate = Builders<User>.Update.PullFilter(m => m.Teams, t => t.Id == team.Id);
            var pushUpdate = Builders<User>.Update.Push(m => m.Teams, team);
            var requests = new[]
            {
                new UpdateOneModel<User>(filter, pullUpdate),
                new UpdateOneModel<User>(filter, pushUpdate)
            };

            await _usersCollection.BulkWriteAsync(requests, cancellationToken: cancellationToken);
        }
    }
}