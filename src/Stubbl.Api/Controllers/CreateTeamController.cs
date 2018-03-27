using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Filters;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using Stubbl.Api.Commands.CreateTeam.Version1;
using Stubbl.Api.Models.CreateTeam.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/create", Name = "CreateTeam")]
    public class CreateTeamController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public CreateTeamController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateTeamResponse), 201)]
        [SwaggerOperation(Tags = new[] {"Teams"})]
        [ValidateModelState]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateTeamCommand
            (
                request.Name
            );

            var @event = await _commandDispatcher.DispatchAsync(command, cancellationToken);

            var location = Url.RouteUrl("FindTeam", new {teamId = @event.TeamId}, null, Request.Host.Value);

            Response.Headers["Location"] = location;

            var response = new CreateTeamResponse(@event.TeamId.ToString());

            return StatusCode(201, response);
        }
    }
}