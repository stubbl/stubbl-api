using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Authentication;
using Gunnsoft.Cqs.CommandHandlers;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.UpdateTeamStub.Version1;
using Stubbl.Api.Data.Collections.Shared;
using Stubbl.Api.Data.Collections.Stubs;
using Stubbl.Api.Events.TeamStubUpdated.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageStubs.Version1;
using Stubbl.Api.Exceptions.MemberNotAddedToTeam.Version1;
using Stubbl.Api.Exceptions.StubNotFound.Version1;
using Request = Stubbl.Api.Events.Shared.Version1.Request;
using Response = Stubbl.Api.Events.Shared.Version1.Response;

namespace Stubbl.Api.CommandHandlers
{
    public class UpdateTeamStubCommandHandler : ICommandHandler<UpdateTeamStubCommand, TeamStubUpdatedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Stub> _stubsCollection;

        public UpdateTeamStubCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Stub> stubsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _stubsCollection = stubsCollection;
        }

        public async Task<TeamStubUpdatedEvent> HandleAsync(UpdateTeamStubCommand command,
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

            if (!await _stubsCollection.Find(s => s.TeamId == team.Id && s.Id == command.StubId)
                .AnyAsync(cancellationToken))
            {
                throw new StubNotFoundException
                (
                    command.StubId,
                    team.Id
                );
            }

            var filter = Builders<Stub>.Filter.Where(s => s.Id == command.StubId);
            var update = Builders<Stub>.Update.Set(s => s.Name, command.Name)
                .Set(s => s.Request.HttpMethod, command.Request.HttpMethod)
                .Set(s => s.Request.Path, command.Request.Path)
                .Set(s => s.Request.QueryStringParameters,
                    command.Request.QueryStringParameters
                        ?.Select(qcc => new QueryStringParameter {Key = qcc.Key, Value = qcc.Value}).ToList())
                .Set(s => s.Request.BodyTokens,
                    command.Request.BodyTokens
                        ?.Select(bt => new BodyToken {Path = bt.Path, Type = bt.Type, Value = bt.Value}).ToList())
                .Set(s => s.Request.Headers,
                    command.Request.Headers?.Select(h => new Header {Key = h.Key, Value = h.Value}).ToList())
                .Set(s => s.Response.HttpStatusCode, command.Response.HttpStatusCode)
                .Set(s => s.Response.Body, command.Response.Body)
                .Set(s => s.Response.Headers,
                    command.Response.Headers?.Select(h => new Header {Key = h.Key, Value = h.Value}).ToList())
                .Set(s => s.Tags, command.Tags);

            await _stubsCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

            return new TeamStubUpdatedEvent
            (
                command.StubId,
                command.TeamId,
                command.Name,
                new Request
                (
                    command.Request.HttpMethod,
                    command.Request.Path,
                    command.Request.QueryStringParameters?.Select(qcc =>
                        new Events.Shared.Version1.QueryStringParameter(qcc.Key, qcc.Value)).ToList(),
                    command.Request.BodyTokens
                        ?.Select(bt => new Events.Shared.Version1.BodyToken(bt.Path, bt.Type, bt.Value)).ToList(),
                    command.Request.Headers?.Select(h => new Events.Shared.Version1.Header(h.Key, h.Value)).ToList()
                ),
                new Response
                (
                    command.Response.HttpStatusCode,
                    command.Response.Body,
                    command.Request.Headers?.Select(h => new Events.Shared.Version1.Header(h.Key, h.Value)).ToList()
                ),
                command.Tags
            );
        }
    }
}