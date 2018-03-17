using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Commands.CloneTeamStub.Version1;
using Stubbl.Api.Filters;
using Stubbl.Api.Models.CloneTeamStub.Version1;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/stubs/{stubId:ObjectId}/clone", Name = "CloneTeamStub")]
    public class CloneTeamStubController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public CloneTeamStubController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CloneTeamStubResponse), 201)]
        [ValidateModelState]
        public async Task<IActionResult> CloneTeamStub([FromRoute] string teamId, [FromRoute] string stubId,
            [FromBody] CloneTeamStubRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CloneTeamStubCommand
            (
                ObjectId.Parse(teamId),
                ObjectId.Parse(stubId),
                request.Name
            );

            var @event = await _commandDispatcher.DispatchAsync(command, cancellationToken);

            var location = Url.RouteUrl("FindTeamStub", new {teamId, stubId = @event.StubId}, null, Request.Host.Value);

            Response.Headers["Location"] = location;

            var response = new CloneTeamStubResponse(@event.StubId.ToString());

            return StatusCode(201, response);
        }
    }
}