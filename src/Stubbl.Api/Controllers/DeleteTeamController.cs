using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Commands.DeleteTeam.Version1;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/delete", Name = "DeleteTeam")]
    public class DeleteTeamController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public DeleteTeamController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), 204)]
        public async Task<IActionResult> DeleteTeam([FromRoute] string teamId, CancellationToken cancellationToken)
        {
            var command = new DeleteTeamCommand
            (
                ObjectId.Parse(teamId)
            );

            await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return StatusCode(204);
        }
    }
}