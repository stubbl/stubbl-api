using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Data.Collections.Users;
using Stubbl.Api.Events.AuthenticatedUserInvitationAccepted.Version1;
using Role = Stubbl.Api.Data.Collections.Users.Role;
using Team = Stubbl.Api.Data.Collections.Users.Team;

namespace Stubbl.Api.EventHandlers.Collections.Members
{
    public class
        AuthenticatedUserInvitationAcceptedEventHandler : IEventHandler<AuthenticatedUserInvitationAcceptedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Invitation> _invitationsCollection;
        private readonly IMongoCollection<User> _usersCollection;

        public AuthenticatedUserInvitationAcceptedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Invitation> invitationsCollection, IMongoCollection<User> usersCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _invitationsCollection = invitationsCollection;
            _usersCollection = usersCollection;
        }

        public async Task HandleAsync(AuthenticatedUserInvitationAcceptedEvent @event,
            CancellationToken cancellationToken)
        {
            var invitation = await _invitationsCollection.Find(i => i.Id == @event.InvitationId)
                .SingleAsync(cancellationToken);

            var team = new Team
            {
                Id = invitation.Team.Id,
                Name = invitation.Team.Name,
                Role = new Role
                {
                    Id = invitation.Role.Id,
                    Name = invitation.Role.Name
                }
            };

            var authenticatedUserId = _authenticatedUserAccessor.AuthenticatedUser.Id;
            var filter = Builders<User>.Filter.Where(m => m.Id == authenticatedUserId);
            var update = Builders<User>.Update.Push(m => m.Teams, team);

            await _usersCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        }
    }
}