using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Filters;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Commands.AcceptAuthenticatedUserInvitation.Version1;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("user/invitations/{invitationId:ObjectId}/accept", Name = "AcceptTeamInvitation")]
    public class AcceptAuthenticatedUserInvitationController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public AcceptAuthenticatedUserInvitationController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), 204)]
        [ValidateModelState]
        public async Task<IActionResult> AcceptAuthenticatedUserInvitation([FromRoute] string invitationId,
            CancellationToken cancellationToken)
        {
            var command = new AcceptAuthenticatedUserInvitationCommand
            (
                ObjectId.Parse(invitationId)
            );

            await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return StatusCode(204);
        }
    }
}