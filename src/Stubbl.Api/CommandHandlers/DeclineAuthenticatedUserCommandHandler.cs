using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Authentication;
using Gunnsoft.Cqs.CommandHandlers;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.DeclineAuthenticatedUserInvitation.Version1;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Events.AuthenticatedUserInvitationDeclined.Version1;
using Stubbl.Api.Exceptions.InvitationAlreadyUsed.Version1;
using Stubbl.Api.Exceptions.MemberAlreadyAddedToTeam.Version1;
using Stubbl.Api.Exceptions.MemberNotInvitedToTeam.Version1;

namespace Stubbl.Api.CommandHandlers
{
    public class DeclineAuthenticatedUserCommandHandler : ICommandHandler<DeclineAuthenticatedUserInvitationCommand,
        AuthenticatedUserInvitationDeclinedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Invitation> _invitationsCollection;

        public DeclineAuthenticatedUserCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Invitation> invitationsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _invitationsCollection = invitationsCollection;
        }

        public async Task<AuthenticatedUserInvitationDeclinedEvent> HandleAsync(
            DeclineAuthenticatedUserInvitationCommand command, CancellationToken cancellationToken)
        {
            var invitation = await _invitationsCollection.Find(i =>
                    i.Id == command.InvitationId && i.EmailAddress.ToLower() ==
                    _authenticatedUserAccessor.AuthenticatedUser.EmailAddress.ToLower())
                .SingleOrDefaultAsync(cancellationToken);

            if (invitation == null)
            {
                throw new MemberNotInvitedToTeamException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    command.InvitationId
                );
            }

            if (invitation.Status != InvitationStatus.Sent)
            {
                throw new InvitationAlreadyUsedException
                (
                    invitation.Id,
                    invitation.Team.Id
                );
            }

            if (_authenticatedUserAccessor.AuthenticatedUser.Teams.Any(t => t.Id == invitation.Team.Id))
            {
                throw new MemberAlreadyAddedToTeamException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    invitation.Team.Id
                );
            }

            var filter = Builders<Invitation>.Filter.Where(i => i.Id == invitation.Id);
            var update = Builders<Invitation>.Update.Set(i => i.Status, InvitationStatus.Declined);

            await _invitationsCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

            return new AuthenticatedUserInvitationDeclinedEvent
            (
                invitation.Id
            );
        }
    }
}