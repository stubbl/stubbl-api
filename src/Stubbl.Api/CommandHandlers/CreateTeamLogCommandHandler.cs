using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using MongoDB.Driver;
using Stubbl.Api.Commands.CreateTeamLog.Version1;
using Stubbl.Api.Data.Collections.Logs;
using Stubbl.Api.Data.Collections.Shared;
using Stubbl.Api.Events.TeamLogCreated.Version1;
using Request = Stubbl.Api.Data.Collections.Logs.Request;
using Response = Stubbl.Api.Data.Collections.Logs.Response;

namespace Stubbl.Api.CommandHandlers
{
    public class CreateTeamLogCommandHandler : ICommandHandler<CreateTeamLogCommand, TeamLogCreatedEvent>
    {
        private readonly IMongoCollection<Log> _logsCollection;

        public CreateTeamLogCommandHandler(IMongoCollection<Log> logsCollection)
        {
            _logsCollection = logsCollection;
        }

        public async Task<TeamLogCreatedEvent> HandleAsync(CreateTeamLogCommand command,
            CancellationToken cancellationToken)
        {
            var log = new Log
            {
                Id = ObjectId.GenerateNewId(),
                TeamId = command.TeamId,
                StubIds = command.StubIds?.ToList(),
                Request = new Request
                {
                    HttpMethod = command.Request.HttpMethod,
                    Path = command.Request.Path,
                    QueryStringParameters = command.Request.QueryStringParameters
                        ?.Select(qp => new QueryStringParameter {Key = qp.Key, Value = qp.Value}).ToList(),
                    Body = command.Request.Body,
                    Headers = command.Request.Headers?.Select(qp => new Header {Key = qp.Key, Value = qp.Value})
                        .ToList()
                },
                Response = new Response
                {
                    HttpStatusCode = command.Response.HttpStatusCode,
                    Body = command.Response.Body,
                    Headers = command.Response.Headers?.Select(qp => new Header {Key = qp.Key, Value = qp.Value})
                        .ToList()
                }
            };

            await _logsCollection.InsertOneAsync(log, cancellationToken: cancellationToken);

            return new TeamLogCreatedEvent
            (
                log.TeamId,
                log.Id,
                log.StubIds,
                new Events.TeamLogCreated.Version1.Request
                (
                    log.Request.HttpMethod,
                    log.Request.Path,
                    log.Request.QueryStringParameters
                        ?.Select(qp => new Events.Shared.Version1.QueryStringParameter(qp.Key, qp.Value)).ToList(),
                    log.Request.Body,
                    log.Request.Headers?.Select(qp => new Events.Shared.Version1.Header(qp.Key, qp.Value)).ToList()
                ),
                new Events.TeamLogCreated.Version1.Response
                (
                    log.Response.HttpStatusCode,
                    log.Response.Body,
                    log.Response.Headers?.Select(qp => new Events.Shared.Version1.Header(qp.Key, qp.Value)).ToList()
                )
            );
        }
    }
}