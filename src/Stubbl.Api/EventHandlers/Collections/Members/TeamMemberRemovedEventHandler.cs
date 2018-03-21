using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Data.Collections.Users;
using Stubbl.Api.Events.TeamMemberRemoved.Version1;

namespace Stubbl.Api.EventHandlers.Collections.Members
{
    public class TeamMemberRemovedEventHandler : IEventHandler<TeamMemberRemovedEvent>
    {
        private readonly IMongoCollection<User> _usersCollection;

        public TeamMemberRemovedEventHandler(IMongoCollection<User> usersCollection)
        {
            _usersCollection = usersCollection;
        }

        public async Task HandleAsync(TeamMemberRemovedEvent @event, CancellationToken cancellationToken)
        {
            var filter = Builders<User>.Filter.Where(m => m.Id == @event.MemberId);
            var update = Builders<User>.Update.PullFilter(t => t.Teams, m => m.Id == @event.TeamId);

            await _usersCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        }
    }
}