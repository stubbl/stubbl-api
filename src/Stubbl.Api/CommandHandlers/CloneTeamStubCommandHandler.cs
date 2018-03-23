using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.CloneTeamStub.Version1;
using Stubbl.Api.Data.Collections.Shared;
using Stubbl.Api.Data.Collections.Stubs;
using Stubbl.Api.Events.TeamStubCloned.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageStubs.Version1;
using Stubbl.Api.Exceptions.StubNotFound.Version1;
using Stubbl.Api.Exceptions.UserNotAddedToTeam.Version1;
using BodyToken = Stubbl.Api.Events.Shared.Version1.BodyToken;
using Header = Stubbl.Api.Events.Shared.Version1.Header;
using QueryStringParameter = Stubbl.Api.Events.Shared.Version1.QueryStringParameter;
using Request = Stubbl.Api.Events.Shared.Version1.Request;
using Response = Stubbl.Api.Events.Shared.Version1.Response;

namespace Stubbl.Api.CommandHandlers
{
    public class CloneTeamStubCommandHandler : ICommandHandler<CloneTeamStubCommand, TeamStubClonedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Stub> _stubsCollection;

        public CloneTeamStubCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Stub> stubsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _stubsCollection = stubsCollection;
        }

        public async Task<TeamStubClonedEvent> HandleAsync(CloneTeamStubCommand command,
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

            if (!team.Role.Permissions.Contains(Permission.ManageStubs))
            {
                throw new MemberCannotManageStubsException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    team.Id
                );
            }

            var stub = await _stubsCollection.Find(s => s.TeamId == team.Id && s.Id == command.StubId)
                .SingleOrDefaultAsync(cancellationToken);

            if (stub == null)
            {
                throw new StubNotFoundException
                (
                    command.StubId,
                    team.Id
                );
            }

            var clonedStub = stub;
            clonedStub.Id = ObjectId.GenerateNewId();
            clonedStub.Name = command.Name;

            await _stubsCollection.InsertOneAsync(stub, cancellationToken: cancellationToken);

            return new TeamStubClonedEvent
            (
                clonedStub.Id,
                clonedStub.TeamId,
                clonedStub.Name,
                new Request
                (
                    clonedStub.Request.HttpMethod,
                    clonedStub.Request.Path,
                    clonedStub.Request.QueryStringParameters
                        ?.Select(qcc => new QueryStringParameter(qcc.Key, qcc.Value)).ToList(),
                    clonedStub.Request.BodyTokens?.Select(bt => new BodyToken(bt.Path, bt.Type, bt.Value)).ToList(),
                    clonedStub.Request.Headers?.Select(h => new Header(h.Key, h.Value)).ToList()
                ),
                new Response
                (
                    clonedStub.Response.HttpStatusCode,
                    clonedStub.Response.Body,
                    clonedStub.Request.Headers?.Select(h => new Header(h.Key, h.Value)).ToList()
                ),
                clonedStub.Tags
            );
        }
    }
}