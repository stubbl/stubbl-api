using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.AcceptAuthenticatedUserTeamInvitation.Version1;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Events.TeamInvitationAccepted.Version1;
using Stubbl.Api.Exceptions.InvitationAlreadyUsed.Version1;
using Stubbl.Api.Exceptions.MemberAlreadyAddedToTeam.Version1;
using Stubbl.Api.Exceptions.RoleNotFound.Version1;
using Stubbl.Api.Exceptions.TeamNotFound.Version1;
using Stubbl.Api.Exceptions.UserNotInvitedToTeam.Version1;
using Team = Stubbl.Api.Data.Collections.Teams.Team;

namespace Stubbl.Api.CommandHandlers
{
    public class AcceptAuthenticatedUserTeamInvitationCommandHandler : ICommandHandler<
        AcceptAuthenticatedUserTeamInvitationCommand, TeamInvitationAcceptedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Invitation> _invitationsCollection;
        private readonly IMongoCollection<Team> _teamsCollection;

        public AcceptAuthenticatedUserTeamInvitationCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Invitation> invitationsCollection, IMongoCollection<Team> teamsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _invitationsCollection = invitationsCollection;
            _teamsCollection = teamsCollection;
        }

        public async Task<TeamInvitationAcceptedEvent> HandleAsync(
            AcceptAuthenticatedUserTeamInvitationCommand command, CancellationToken cancellationToken)
        {
            var invitation = await _invitationsCollection.Find(i =>
                    i.Id == command.InvitationId && i.EmailAddress.ToLower() ==
                    _authenticatedUserAccessor.AuthenticatedUser.EmailAddress.ToLower())
                .SingleOrDefaultAsync(cancellationToken);

            if (invitation == null)
            {
                throw new UserNotInvitedToTeamException
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

            var team = await _teamsCollection.Find(t => t.Id == invitation.Team.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (team == null)
            {
                throw new TeamNotFoundException
                (
                    invitation.Team.Id
                );
            }

            if (!team.Roles.Any(r => r.Id == invitation.Role.Id))
            {
                throw new RoleNotFoundException
                (
                    invitation.Role.Id,
                    invitation.Team.Id
                );
            }

            var filter = Builders<Invitation>.Filter.Where(i => i.Id == command.InvitationId);
            var update = Builders<Invitation>.Update.Set(i => i.Status, InvitationStatus.Accepted);

            await _invitationsCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

            return new TeamInvitationAcceptedEvent
            (
                invitation.Id
            );
        }
    }
}