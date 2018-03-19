using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Data.Collections.Logs;
using Stubbl.Api.Events.TeamDeleted.Version1;

namespace Stubbl.Api.EventHandlers.Collections.Logs
{
    public class TeamDeletedEventHandler : IEventHandler<TeamDeletedEvent>
    {
        private readonly IMongoCollection<Log> _logsCollection;

        public TeamDeletedEventHandler(IMongoCollection<Log> logsCollection)
        {
            _logsCollection = logsCollection;
        }

        public async Task HandleAsync(TeamDeletedEvent @event, CancellationToken cancellationToken)
        {
            var filter = Builders<Log>.Filter.Where(m => m.TeamId == @event.TeamId);

            await _logsCollection.DeleteManyAsync(filter, cancellationToken);
        }
    }
}