using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Commands.DeclineAuthenticatedUserInvitation.Version1;
using Stubbl.Api.Filters;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("invitations/{invitationId:ObjectId}/decline", Name = "DeclineTeamInvitation")]
    public class DeclineAuthenticatedUserInvitationController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public DeclineAuthenticatedUserInvitationController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), 204)]
        [ValidateModelState]
        public async Task<IActionResult> DeclineAuthenticatedUserInvitation([FromRoute] string invitationId,
            CancellationToken cancellationToken)
        {
            var command = new DeclineAuthenticatedUserInvitationCommand
            (
                ObjectId.Parse(invitationId)
            );

            await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return StatusCode(204);
        }
    }
}