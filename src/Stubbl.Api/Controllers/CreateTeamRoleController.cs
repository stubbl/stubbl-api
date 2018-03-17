using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Filters;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Commands.CreateTeamRole.Version1;
using Stubbl.Api.Commands.Shared.Version1;
using Stubbl.Api.Models.CreateTeamRole.Version1;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/roles/create", Name = "CreateTeamRole")]
    public class CreateTeamRoleController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public CreateTeamRoleController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateTeamRoleResponse), 201)]
        [ValidateModelState]
        public async Task<IActionResult> CreateTeamRole([FromRoute] string teamId,
            [FromBody] CreateTeamRoleRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateTeamRoleCommand
            (
                ObjectId.Parse(teamId),
                request.Name,
                request.Permissions.Select(p => (Permission) Enum.Parse(typeof(Permission), p)).ToList()
            );

            var @event = await _commandDispatcher.DispatchAsync(command, cancellationToken);

            var location = Url.RouteUrl("FindTeamRole", new {teamId, roleId = @event.RoleId}, null, Request.Host.Value);

            Response.Headers["Location"] = location;

            var response = new CreateTeamRoleResponse(@event.RoleId.ToString());

            return StatusCode(201, response);
        }
    }
}