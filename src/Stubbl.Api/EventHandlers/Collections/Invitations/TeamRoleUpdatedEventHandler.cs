using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Events.TeamRoleUpdated.Version1;

namespace Stubbl.Api.EventHandlers.Collections.Invitations
{
    public class TeamRoleUpdatedEventHandler : IEventHandler<TeamRoleUpdatedEvent>
    {
        private readonly IMongoCollection<Invitation> _invitationsCollection;

        public TeamRoleUpdatedEventHandler(IMongoCollection<Invitation> invitationsCollection)
        {
            _invitationsCollection = invitationsCollection;
        }

        public async Task HandleAsync(TeamRoleUpdatedEvent @event, CancellationToken cancellationToken)
        {
            var filter = Builders<Invitation>.Filter.Where(m => m.Role.Id == @event.RoleId);
            var update = Builders<Invitation>.Update.Set(m => m.Role.Name, @event.Name);

            await _invitationsCollection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        }
    }
}