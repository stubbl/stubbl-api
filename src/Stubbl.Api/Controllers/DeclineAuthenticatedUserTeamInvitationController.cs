using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Filters;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Commands.DeclineAuthenticatedUserTeamInvitation.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("user/team-invitations/{invitationId:ObjectId}/decline", Name = "DeclineAuthenticatedUserTeamInvitation")]
    public class DeclineAuthenticatedUserTeamInvitationController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public DeclineAuthenticatedUserTeamInvitationController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), 204)]
        [ValidateModelState]
        [SwaggerOperation(Tags = new[] {"Authenticated User Team Invitations"})]
        public async Task<IActionResult> DeclineAuthenticatedUserTeamInvitation([FromRoute] string invitationId,
            CancellationToken cancellationToken)
        {
            var command = new DeclineAuthenticatedUserTeamInvitationCommand
            (
                ObjectId.Parse(invitationId)
            );

            await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return StatusCode(204);
        }
    }
}