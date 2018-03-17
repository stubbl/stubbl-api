using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Filters;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Commands.Shared.Version1;
using Stubbl.Api.Commands.UpdateTeamRole.Version1;
using Stubbl.Api.Models.UpdateTeamRole.Version1;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/roles/{roleId:ObjectId}/update", Name = "UpdateTeamRole")]
    public class UpdateTeamRoleController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public UpdateTeamRoleController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), 204)]
        [ValidateModelState]
        public async Task<IActionResult> UpdateTeamRole([FromRoute] string teamId, [FromRoute] string roleId,
            [FromBody] UpdateTeamRoleRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateTeamRoleCommand
            (
                ObjectId.Parse(teamId),
                ObjectId.Parse(roleId),
                request.Name,
                request.Permissions.Select(p => (Permission) Enum.Parse(typeof(Permission), p)).ToList()
            );

            await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return StatusCode(204);
        }
    }
}