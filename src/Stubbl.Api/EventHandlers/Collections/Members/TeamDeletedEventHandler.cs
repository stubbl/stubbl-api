using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Data.Collections.Users;
using Stubbl.Api.Events.TeamDeleted.Version1;

namespace Stubbl.Api.EventHandlers.Collections.Members
{
    public class TeamDeletedEventHandler : IEventHandler<TeamDeletedEvent>
    {
        private readonly IMongoCollection<User> _usersCollection;

        public TeamDeletedEventHandler(IMongoCollection<User> usersCollection)
        {
            _usersCollection = usersCollection;
        }

        public async Task HandleAsync(TeamDeletedEvent @event, CancellationToken cancellationToken)
        {
            var filter = Builders<User>.Filter.Where(m => m.Teams.Any(t => t.Id == @event.TeamId));
            var update = Builders<User>.Update.PullFilter(m => m.Teams, t => t.Id == @event.TeamId);

            await _usersCollection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        }
    }
}