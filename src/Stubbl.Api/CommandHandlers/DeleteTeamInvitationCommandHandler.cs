using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.CommandHandlers;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.DeleteTeamInvitation.Version1;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Data.Collections.Shared;
using Stubbl.Api.Events.TeamInvitationDeleted.Version1;
using Stubbl.Api.Exceptions.InvitationNotFound.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageInvitations.Version1;
using Stubbl.Api.Exceptions.MemberNotAddedToTeam.Version1;

namespace Stubbl.Api.CommandHandlers
{
    public class
        DeleteTeamInvitationCommandHandler : ICommandHandler<DeleteTeamInvitationCommand, TeamInvitationDeletedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Invitation> _invitationsCollection;

        public DeleteTeamInvitationCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Invitation> invitationsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _invitationsCollection = invitationsCollection;
        }

        public async Task<TeamInvitationDeletedEvent> HandleAsync(DeleteTeamInvitationCommand command,
            CancellationToken cancellationToken)
        {
            var team = _authenticatedUserAccessor.AuthenticatedUser.Teams.SingleOrDefault(t => t.Id == command.TeamId);

            if (team == null)
            {
                throw new MemberNotAddedToTeamException
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

            var filter = Builders<Invitation>.Filter.Where(i => i.Id == command.InvitationId && i.Team.Id == team.Id);

            var result = await _invitationsCollection.DeleteOneAsync(filter, cancellationToken);

            if (result.DeletedCount == 0)
            {
                throw new InvitationNotFoundException
                (
                    command.InvitationId,
                    team.Id
                );
            }

            return new TeamInvitationDeletedEvent
            (
                team.Id,
                command.InvitationId
            );
        }
    }
}