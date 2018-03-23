using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.CreateTeamInvitation.Version1;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Data.Collections.Shared;
using Stubbl.Api.Data.Collections.Users;
using Stubbl.Api.Events.TeamInvitationCreated.Version1;
using Stubbl.Api.Exceptions.MemberAlreadyAddedToTeam.Version1;
using Stubbl.Api.Exceptions.MemberAlreadyInvitedToTeam.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageInvitations.Version1;
using Stubbl.Api.Exceptions.RoleNotFound.Version1;
using Stubbl.Api.Exceptions.UserNotAddedToTeam.Version1;
using Role = Stubbl.Api.Data.Collections.Invitations.Role;
using Team = Stubbl.Api.Data.Collections.Teams.Team;

namespace Stubbl.Api.CommandHandlers
{
    public class
        CreateTeamInvitationCommandHandler : ICommandHandler<CreateTeamInvitationCommand, TeamInvitationCreatedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Invitation> _invitationsCollection;
        private readonly IMongoCollection<Team> _teamsCollection;
        private readonly IMongoCollection<User> _usersCollection;

        public CreateTeamInvitationCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Invitation> invitationsCollection, IMongoCollection<Team> teamsCollection,
            IMongoCollection<User> usersCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _invitationsCollection = invitationsCollection;
            _teamsCollection = teamsCollection;
            _usersCollection = usersCollection;
        }

        public async Task<TeamInvitationCreatedEvent> HandleAsync(CreateTeamInvitationCommand command,
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

            if (string.Equals(_authenticatedUserAccessor.AuthenticatedUser.EmailAddress, command.EmailAddress,
                StringComparison.OrdinalIgnoreCase))
            {
                throw new MemberAlreadyAddedToTeamException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    team.Id
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

            var user = await _usersCollection.Find(m => m.EmailAddress.ToLower() == command.EmailAddress.ToLower())
                .SingleOrDefaultAsync(cancellationToken);

            if (user != null && user.Teams.Any(t => t.Id == team.Id))
            {
                throw new MemberAlreadyAddedToTeamException
                (
                    user.Id,
                    team.Id
                );
            }

            if (await _invitationsCollection
                .Find(i => i.Team.Id == team.Id && i.EmailAddress.ToLower() == command.EmailAddress.ToLower() &&
                           i.Status == InvitationStatus.Sent).AnyAsync(cancellationToken))
            {
                throw new MemberAlreadyInvitedToTeamException
                (
                    user.Id,
                    team.Id
                );
            }

            var role = await _teamsCollection.Find(t => t.Id == team.Id && t.Roles.Any(r => r.Id == command.RoleId))
                .Project(t => t.Roles.SingleOrDefault(r => r.Id == command.RoleId))
                .SingleOrDefaultAsync(cancellationToken);

            if (role == null)
            {
                throw new RoleNotFoundException
                (
                    command.RoleId,
                    team.Id
                );
            }

            var invitation = new Invitation
            {
                Role = new Role
                {
                    Id = role.Id,
                    Name = role.Name
                },
                Team = new Data.Collections.Invitations.Team
                {
                    Id = team.Id,
                    Name = team.Name
                },
                EmailAddress = command.EmailAddress,
                Status = InvitationStatus.Sent
            };

            await _invitationsCollection.InsertOneAsync(invitation, cancellationToken: cancellationToken);

            return new TeamInvitationCreatedEvent
            (
                invitation.Id,
                invitation.Team.Id,
                invitation.Role.Id,
                invitation.EmailAddress
            );
        }
    }
}