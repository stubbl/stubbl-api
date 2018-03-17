using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Commands.CreateTeamInvitation.Version1;
using Stubbl.Api.Filters;
using Stubbl.Api.Models.CreateTeamInvitation.Version1;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/invitations/create", Name = "CreateTeamInvitation")]
    public class CreateTeamInvitationController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public CreateTeamInvitationController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), 204)]
        [ValidateModelState]
        public async Task<IActionResult> CreateTeamInvitation([FromRoute] string teamId, [FromRoute] string roleId,
            [FromBody] CreateTeamInvitationRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateTeamInvitationCommand
            (
                ObjectId.Parse(teamId),
                ObjectId.Parse(roleId),
                request.EmailAddress
            );

            var @event = await _commandDispatcher.DispatchAsync(command, cancellationToken);

            var location = Url.RouteUrl("FindTeamInvitation", new {teamId, inviationId = @event.InvitationId}, null,
                Request.Host.Value);

            Response.Headers["Location"] = location;

            var response = new CreateTeamInvitationResponse(@event.InvitationId.ToString());

            return StatusCode(201, response);
        }
    }
}