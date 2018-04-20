using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Filters;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Commands.AcceptAuthenticatedUserTeamInvitation.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("user/team-invitations/{invitationId:ObjectId}/accept", Name = "AcceptAuthenticatedUserTeamInvitation")]
    public class AcceptAuthenticatedUserTeamInvitationController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public AcceptAuthenticatedUserTeamInvitationController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), 204)]
        [SwaggerOperation(Tags = new[] {"Authenticated User Team Invitations"})]
        [ValidateModelState]
        public async Task<IActionResult> AcceptAuthenticatedUserTeamInvitation([FromRoute] string invitationId,
            CancellationToken cancellationToken)
        {
            var command = new AcceptAuthenticatedUserTeamInvitationCommand
            (
                ObjectId.Parse(invitationId)
            );

            await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return StatusCode(204);
        }
    }
}