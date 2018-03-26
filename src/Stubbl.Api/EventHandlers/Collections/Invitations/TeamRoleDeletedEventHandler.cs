using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Data;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Events.TeamRoleDeleted.Version1;
using Team = Stubbl.Api.Data.Collections.Teams.Team;

namespace Stubbl.Api.EventHandlers.Collections.Invitations
{
    public class TeamRoleDeletedEventHandler : IEventHandler<TeamRoleDeletedEvent>
    {
        private readonly IMongoCollection<Invitation> _invitationsCollection;
        private readonly IMongoCollection<Team> _teamsCollection;

        public TeamRoleDeletedEventHandler(IMongoCollection<Invitation> invitationsCollection,
            IMongoCollection<Team> teamsCollection)
        {
            _invitationsCollection = invitationsCollection;
            _teamsCollection = teamsCollection;
        }

        public async Task HandleAsync(TeamRoleDeletedEvent @event, CancellationToken cancellationToken)
        {
            var userRole = await _teamsCollection.Find(t => t.Id == @event.TeamId)
                .Project(t => t.Roles.Single(r => r.Name.ToLower() == DefaultRoles.User.Name.ToLower() && r.IsDefault))
                .SingleAsync(cancellationToken);

            var filter =
                Builders<Invitation>.Filter.Where(i => i.Team.Id == @event.TeamId && i.Role.Id == @event.RoleId);
            var update = Builders<Invitation>.Update.Set(i => i.Role.Id, userRole.Id)
                .Set(i => i.Role.Name, userRole.Name);

            await _invitationsCollection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        }
    }
}