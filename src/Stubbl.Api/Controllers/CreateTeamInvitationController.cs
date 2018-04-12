using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Filters;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Commands.CreateTeamInvitation.Version1;
using Stubbl.Api.Models.CreateTeamInvitation.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

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
        [ProducesResponseType(typeof(CreateTeamInvitationResponse), 201)]
        [SwaggerOperation(Tags = new[] {"Team Invitations"})]
        [ValidateModelState]
        public async Task<IActionResult> CreateTeamInvitation([FromRoute] string teamId, [FromBody] CreateTeamInvitationRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateTeamInvitationCommand
            (
                ObjectId.Parse(teamId),
                ObjectId.Parse(request.RoleId),
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