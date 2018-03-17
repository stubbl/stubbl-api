using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Filters;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Commands.DeleteTeamRole.Version1;
using Stubbl.Api.Models.CreateTeamRole.Version1;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/roles/{roleId:ObjectId}/delete", Name = "DeleteTeamRole")]
    public class DeleteTeamRoleController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public DeleteTeamRoleController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateTeamRoleResponse), 201)]
        [ValidateModelState]
        public async Task<IActionResult> DeleteTeamRole([FromRoute] string teamId, [FromRoute] string roleId,
            CancellationToken cancellationToken)
        {
            var command = new DeleteTeamRoleCommand
            (
                ObjectId.Parse(teamId),
                ObjectId.Parse(roleId)
            );

            await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return StatusCode(204);
        }
    }
}