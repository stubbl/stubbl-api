using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Filters;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Commands.CreateTeamStub.Version1;
using Stubbl.Api.Commands.Shared.Version1;
using Stubbl.Api.Models.CreateTeamStub.Version1;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/stubs/create", Name = "CreateTeamStub")]
    public class CreateTeamStubController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public CreateTeamStubController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateTeamStubResponse), 201)]
        [ValidateModelState]
        public async Task<IActionResult> CreateTeamStub([FromRoute] string teamId,
            [FromBody] CreateTeamStubRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateTeamStubCommand
            (
                ObjectId.Parse(teamId),
                request.Name,
                new Request
                (
                    request.Request.HttpMethod,
                    request.Request.Path,
                    request.Request.QueryStringParameters?.Select(qcc => new QueryStringParameter(qcc.Key, qcc.Value))
                        .ToList(),
                    request.Request.BodyTokens?.Select(bt => new BodyToken(bt.Path, bt.Type, bt.Value)).ToList(),
                    request.Request.Headers?.Select(h => new Header(h.Key, h.Value)).ToList()
                ),
                new Response
                (
                    request.Response.HttpStatusCode.Value,
                    request.Response.Body,
                    request.Request.Headers?.Select(h => new Header(h.Key, h.Value)).ToList()
                ),
                request.Tags
            );

            var @event = await _commandDispatcher.DispatchAsync(command, cancellationToken);

            var location = Url.RouteUrl("FindTeamStub", new {teamId, stubId = @event.StubId}, null, Request.Host.Value);

            Response.Headers["Location"] = location;

            var response = new CreateTeamStubResponse(@event.StubId.ToString());

            return StatusCode(201, response);
        }
    }
}