using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.EventHandlers;
using MongoDB.Driver;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Events.TeamUpdated.Version1;

namespace Stubbl.Api.EventHandlers.Collections.Invitations
{
    public class TeamUpdatedEventHandler : IEventHandler<TeamUpdatedEvent>
    {
        private readonly IMongoCollection<Invitation> _invitationsCollection;

        public TeamUpdatedEventHandler(IMongoCollection<Invitation> invitationsCollection)
        {
            _invitationsCollection = invitationsCollection;
        }

        public async Task HandleAsync(TeamUpdatedEvent @event, CancellationToken cancellationToken)
        {
            var filter = Builders<Invitation>.Filter.Where(m => m.Team.Id == @event.TeamId);
            var update = Builders<Invitation>.Update.Set(m => m.Team.Name, @event.Name);

            await _invitationsCollection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        }
    }
}