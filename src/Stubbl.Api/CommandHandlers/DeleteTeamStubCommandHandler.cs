using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Authentication;
using Gunnsoft.Cqs.CommandHandlers;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.DeleteTeamStub.Version1;
using Stubbl.Api.Data.Collections.Shared;
using Stubbl.Api.Data.Collections.Stubs;
using Stubbl.Api.Events.TeamStubDeleted.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageStubs.Version1;
using Stubbl.Api.Exceptions.MemberNotAddedToTeam.Version1;
using Stubbl.Api.Exceptions.StubNotFound.Version1;

namespace Stubbl.Api.CommandHandlers
{
    public class DeleteTeamStubCommandHandler : ICommandHandler<DeleteTeamStubCommand, TeamStubDeletedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Stub> _stubsCollection;

        public DeleteTeamStubCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Stub> stubsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _stubsCollection = stubsCollection;
        }

        public async Task<TeamStubDeletedEvent> HandleAsync(DeleteTeamStubCommand command,
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

            if (!team.Role.Permissions.Contains(Permission.ManageStubs))
            {
                throw new MemberCannotManageStubsException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    team.Id
                );
            }

            var filter = Builders<Stub>.Filter.Where(t => t.Id == command.StubId);

            var result = await _stubsCollection.DeleteOneAsync(filter, cancellationToken);

            if (result.DeletedCount == 0)
            {
                throw new StubNotFoundException
                (
                    command.StubId,
                    team.Id
                );
            }

            return new TeamStubDeletedEvent
            (
                team.Id,
                command.StubId
            );
        }
    }
}