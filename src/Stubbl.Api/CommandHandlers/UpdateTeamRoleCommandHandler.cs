using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.Shared.Version1;
using Stubbl.Api.Commands.UpdateTeamRole.Version1;
using Stubbl.Api.Data.Collections.Teams;
using Stubbl.Api.Events.TeamRoleUpdated.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageTeams.Version1;
using Stubbl.Api.Exceptions.RoleCannotBeUpdated.Version1;
using Stubbl.Api.Exceptions.RoleNotFound.Version1;
using Stubbl.Api.Exceptions.UserNotAddedToTeam.Version1;
using Permission = Stubbl.Api.Data.Collections.Shared.Permission;

namespace Stubbl.Api.CommandHandlers
{
    public class UpdateTeamRoleCommandHandler : ICommandHandler<UpdateTeamRoleCommand, TeamRoleUpdatedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Team> _teamsCollection;

        public UpdateTeamRoleCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Team> teamsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _teamsCollection = teamsCollection;
        }

        public async Task<TeamRoleUpdatedEvent> HandleAsync(UpdateTeamRoleCommand command,
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

            if (!team.Role.Permissions.Contains(Permission.ManageRoles))
            {
                throw new MemberCannotManageTeamsException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    team.Id
                );
            }

            var role = await _teamsCollection.Find(t => t.Id == command.TeamId)
                .Project(t => t.Roles.SingleOrDefault(r => r.Id == command.RoleId))
                .SingleOrDefaultAsync(cancellationToken);

            if (role == null)
            {
                throw new RoleNotFoundException
                (
                    role.Id,
                    team.Id
                );
            }

            if (role.IsDefault)
            {
                throw new RoleCannotBeUpdatedException
                (
                    role.Id,
                    team.Id
                );
            }

            role.Name = command.Name;
            role.Permissions = command.Permissions.ToDataPermissions();

            var filter = Builders<Team>.Filter.Where(t => t.Id == team.Id);
            var pullUpdate = Builders<Team>.Update.PullFilter(t => t.Roles, r => r.Id == role.Id);
            var pushUpdate = Builders<Team>.Update.Push(m => m.Roles, role);
            var requests = new[]
            {
                new UpdateOneModel<Team>(filter, pullUpdate),
                new UpdateOneModel<Team>(filter, pushUpdate)
            };

            await _teamsCollection.BulkWriteAsync(requests, cancellationToken: cancellationToken);

            return new TeamRoleUpdatedEvent
            (
                team.Id,
                role.Id,
                command.Name,
                command.Permissions.ToEventPermissions()
            );
        }
    }
}