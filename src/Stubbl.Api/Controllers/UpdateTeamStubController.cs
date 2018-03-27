using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Filters;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Commands.Shared.Version1;
using Stubbl.Api.Commands.UpdateTeamStub.Version1;
using Stubbl.Api.Models.UpdateTeamStub.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/stubs/{stubId:ObjectId}/update", Name = "UpdateTeamStub")]
    public class UpdateTeamStubController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public UpdateTeamStubController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), 204)]
        [SwaggerOperation(Tags = new[] {"Team Stubs"})]
        [ValidateModelState]
        public async Task<IActionResult> UpdateTeamStub([FromRoute] string teamId, [FromRoute] string stubId,
            [FromBody] UpdateTeamStubRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateTeamStubCommand
            (
                ObjectId.Parse(teamId),
                ObjectId.Parse(stubId),
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
                    request.Response.Headers?.Select(h => new Header(h.Key, h.Value)).ToList()
                ),
                request.Tags
            );

            await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return StatusCode(204);
        }
    }
}