using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.CommandHandlers;
using MongoDB.Bson;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.CreateTeamRole.Version1;
using Stubbl.Api.Commands.Shared.Version1;
using Stubbl.Api.Data.Collections.Teams;
using Stubbl.Api.Events.Shared.Version1;
using Stubbl.Api.Events.TeamRoleCreated.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageRoles.Version1;
using Stubbl.Api.Exceptions.MemberNotAddedToTeam.Version1;
using Stubbl.Api.Exceptions.RoleAlreadyExists.Version1;
using Permission = Stubbl.Api.Data.Collections.Shared.Permission;

namespace Stubbl.Api.CommandHandlers
{
    public class CreateTeamRoleCommandHandler : ICommandHandler<CreateTeamRoleCommand, TeamRoleCreatedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Team> _teamsCollection;

        public CreateTeamRoleCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Team> teamsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _teamsCollection = teamsCollection;
        }

        public async Task<TeamRoleCreatedEvent> HandleAsync(CreateTeamRoleCommand command,
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

            if (!team.Role.Permissions.Contains(Permission.ManageRoles))
            {
                throw new MemberCannotManageRolesException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    team.Id
                );
            }

            if (await _teamsCollection
                .Find(t => t.Id == team.Id && t.Roles.Any(r => r.Name.ToLower() == command.Name.ToLower()))
                .AnyAsync(cancellationToken))
            {
                throw new RoleAlreadyExistsException
                (
                    command.Name,
                    team.Id
                );
            }

            var role = new Role
            {
                Id = ObjectId.GenerateNewId(),
                Name = command.Name,
                Permissions = command.Permissions.ToDataPermissions()
            };

            var filter = Builders<Team>.Filter.Where(t => t.Id == team.Id);
            var update = Builders<Team>.Update.Push(t => t.Roles, role);

            await _teamsCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

            return new TeamRoleCreatedEvent
            (
                role.Id,
                team.Id,
                role.Name,
                role.Permissions.ToEventPermissions()
            );
        }
    }
}