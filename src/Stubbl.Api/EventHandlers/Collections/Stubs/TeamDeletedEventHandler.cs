using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.EventHandlers;
using MongoDB.Driver;
using Stubbl.Api.Data.Collections.Stubs;
using Stubbl.Api.Events.TeamDeleted.Version1;

namespace Stubbl.Api.EventHandlers.Collections.Stubs
{
    public class TeamDeletedEventHandler : IEventHandler<TeamDeletedEvent>
    {
        private readonly IMongoCollection<Stub> _stubsCollection;

        public TeamDeletedEventHandler(IMongoCollection<Stub> stubsCollection)
        {
            _stubsCollection = stubsCollection;
        }

        public async Task HandleAsync(TeamDeletedEvent @event, CancellationToken cancellationToken)
        {
            var filter = Builders<Stub>.Filter.Where(m => m.TeamId == @event.TeamId);

            await _stubsCollection.DeleteManyAsync(filter, cancellationToken);
        }
    }
}