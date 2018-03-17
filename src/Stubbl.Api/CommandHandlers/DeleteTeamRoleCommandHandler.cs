using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.CommandHandlers;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.DeleteTeamRole.Version1;
using Stubbl.Api.Data.Collections.Shared;
using Stubbl.Api.Data.Collections.Teams;
using Stubbl.Api.Events.TeamRoleDeleted.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageMembers.Version1;
using Stubbl.Api.Exceptions.MemberNotAddedToTeam.Version1;
using Stubbl.Api.Exceptions.RoleCannotBeUpdated.Version1;
using Stubbl.Api.Exceptions.RoleNotFound.Version1;

namespace Stubbl.Api.CommandHandlers
{
    public class DeleteTeamRoleCommandHandler : ICommandHandler<DeleteTeamRoleCommand, TeamRoleDeletedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Team> _teamsCollection;

        public DeleteTeamRoleCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Team> teamsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _teamsCollection = teamsCollection;
        }

        public async Task<TeamRoleDeletedEvent> HandleAsync(DeleteTeamRoleCommand command,
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

            if (!team.Role.Permissions.Contains(Permission.ManageMembers))
            {
                throw new MemberCannotManageMembersException
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
                    command.RoleId,
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

            var filter = Builders<Team>.Filter.Where(t => t.Id == team.Id);
            var update = Builders<Team>.Update.PullFilter(t => t.Roles, r => r.Id == role.Id);

            await _teamsCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

            return new TeamRoleDeletedEvent
            (
                role.Id,
                team.Id
            );
        }
    }
}