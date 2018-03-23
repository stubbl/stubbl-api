using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.ResendTeamInvitation.Version1;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Data.Collections.Shared;
using Stubbl.Api.Events.TeamInvitationResent.Version1;
using Stubbl.Api.Exceptions.InvitationAlreadyUsed.Version1;
using Stubbl.Api.Exceptions.InvitationNotFound.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageInvitations.Version1;
using Stubbl.Api.Exceptions.UserNotAddedToTeam.Version1;

namespace Stubbl.Api.CommandHandlers
{
    public class
        ResendTeamInvitationCommandHandler : ICommandHandler<ResendTeamInvitationCommand, TeamInvitationResentEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Invitation> _invitationsCollection;

        public ResendTeamInvitationCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Invitation> invitationsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _invitationsCollection = invitationsCollection;
        }

        public async Task<TeamInvitationResentEvent> HandleAsync(ResendTeamInvitationCommand command,
            CancellationToken cancellationToken)
        {
            var team = _authenticatedUserAccessor.AuthenticatedUser.Teams.SingleOrDefault(t => t.Id == command.TeamId);

            if (team == null)
            {
                throw new UserNotAddedToTeamException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    command.TeamId
                );
            }

            if (!team.Role.Permissions.Contains(Permission.ManageInvitations))
            {
                throw new MemberCannotManageInvitationsException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    team.Id
                );
            }

            var invitation = await _invitationsCollection
                .Find(i => i.Team.Id == team.Id && i.Id == command.InvitationId)
                .SingleOrDefaultAsync(cancellationToken);

            if (invitation == null)
            {
                throw new InvitationNotFoundException
                (
                    command.InvitationId,
                    team.Id
                );
            }

            if (invitation.Status != InvitationStatus.Sent)
            {
                throw new InvitationAlreadyUsedException
                (
                    command.InvitationId,
                    invitation.Team.Id
                );
            }

            return new TeamInvitationResentEvent
            (
                team.Id,
                command.InvitationId
            );
        }
    }
}