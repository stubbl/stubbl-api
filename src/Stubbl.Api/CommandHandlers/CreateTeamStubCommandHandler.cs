using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.CreateTeamStub.Version1;
using Stubbl.Api.Data.Collections.Shared;
using Stubbl.Api.Data.Collections.Stubs;
using Stubbl.Api.Events.TeamStubCreated.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageStubs.Version1;
using Stubbl.Api.Exceptions.MemberNotAddedToTeam.Version1;

namespace Stubbl.Api.CommandHandlers
{
    public class CreateTeamStubCommandHandler : ICommandHandler<CreateTeamStubCommand, TeamStubCreatedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Stub> _stubsCollection;

        public CreateTeamStubCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Stub> stubsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _stubsCollection = stubsCollection;
        }

        public async Task<TeamStubCreatedEvent> HandleAsync(CreateTeamStubCommand command,
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

            var stub = new Stub
            {
                Id = ObjectId.GenerateNewId(),
                TeamId = team.Id,
                Name = command.Name,
                Request = new Request
                {
                    HttpMethod = command.Request.HttpMethod,
                    Path = command.Request.Path,
                    QueryStringParameters = command.Request.QueryStringParameters
                        ?.Select(qcc => new QueryStringParameter {Key = qcc.Key, Value = qcc.Value}).ToList(),
                    BodyTokens = command.Request.BodyTokens
                        ?.Select(bt => new BodyToken {Path = bt.Path, Type = bt.Type, Value = bt.Value}).ToList(),
                    Headers = command.Request.Headers?.Select(h => new Header {Key = h.Key, Value = h.Value}).ToList()
                },
                Response = new Response
                {
                    HttpStatusCode = command.Response.HttpStatusCode,
                    Body = command.Response.Body,
                    Headers = command.Request.Headers?.Select(h => new Header {Key = h.Key, Value = h.Value}).ToList()
                },
                Tags = command.Tags
            };

            await _stubsCollection.InsertOneAsync(stub, cancellationToken: cancellationToken);

            return new TeamStubCreatedEvent
            (
                stub.Id,
                stub.TeamId,
                stub.Name,
                new Events.Shared.Version1.Request
                (
                    stub.Request.HttpMethod,
                    stub.Request.Path,
                    stub.Request.QueryStringParameters
                        ?.Select(qcc => new Events.Shared.Version1.QueryStringParameter(qcc.Key, qcc.Value)).ToList(),
                    stub.Request.BodyTokens
                        ?.Select(bt => new Events.Shared.Version1.BodyToken(bt.Path, bt.Type, bt.Value)).ToList(),
                    stub.Request.Headers?.Select(h => new Events.Shared.Version1.Header(h.Key, h.Value)).ToList()
                ),
                new Events.Shared.Version1.Response
                (
                    stub.Response.HttpStatusCode,
                    stub.Response.Body,
                    stub.Request.Headers?.Select(h => new Events.Shared.Version1.Header(h.Key, h.Value)).ToList()
                ),
                stub.Tags
            );
        }
    }
}