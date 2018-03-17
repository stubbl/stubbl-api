using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Authentication;
using Gunnsoft.Cqs.EventHandlers;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Data.Collections.Members;
using Stubbl.Api.Events.AuthenticatedUserInvitationAccepted.Version1;
using Role = Stubbl.Api.Data.Collections.Members.Role;
using Team = Stubbl.Api.Data.Collections.Members.Team;

namespace Stubbl.Api.EventHandlers.Collections.Members
{
    public class
        AuthenticatedUserInvitationAcceptedEventHandler : IEventHandler<AuthenticatedUserInvitationAcceptedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Invitation> _invitationsCollection;
        private readonly IMongoCollection<Member> _membersCollection;

        public AuthenticatedUserInvitationAcceptedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Invitation> invitationsCollection, IMongoCollection<Member> membersCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _invitationsCollection = invitationsCollection;
            _membersCollection = membersCollection;
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

            var filter = Builders<Member>.Filter.Where(m => m.Id == _authenticatedUserAccessor.AuthenticatedUser.Id);
            var update = Builders<Member>.Update.Push(m => m.Teams, team);

            await _membersCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        }
    }
}