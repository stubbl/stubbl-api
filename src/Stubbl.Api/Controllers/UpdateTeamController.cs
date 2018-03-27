using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Filters;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Commands.UpdateTeam.Version1;
using Stubbl.Api.Models.UpdateTeam.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/update", Name = "UpdateTeam")]
    public class UpdateTeamController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public UpdateTeamController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), 204)]
        [SwaggerOperation(Tags = new[] {"Teams"})]
        [ValidateModelState]
        public async Task<IActionResult> UpdateTeam([FromRoute] string teamId, [FromBody] UpdateTeamRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateTeamCommand
            (
                ObjectId.Parse(teamId),
                request.Name
            );

            await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return StatusCode(204);
        }
    }
}