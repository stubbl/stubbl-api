using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Data.Collections.Members;
using Stubbl.Api.Events.TeamDeleted.Version1;

namespace Stubbl.Api.EventHandlers.Collections.Members
{
    public class TeamDeletedEventHandler : IEventHandler<TeamDeletedEvent>
    {
        private readonly IMongoCollection<Member> _membersCollection;

        public TeamDeletedEventHandler(IMongoCollection<Member> membersCollection)
        {
            _membersCollection = membersCollection;
        }

        public async Task HandleAsync(TeamDeletedEvent @event, CancellationToken cancellationToken)
        {
            var filter = Builders<Member>.Filter.Where(m => m.Teams.Any(t => t.Id == @event.TeamId));
            var update = Builders<Member>.Update.PullFilter(m => m.Teams, t => t.Id == @event.TeamId);

            await _membersCollection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        }
    }
}