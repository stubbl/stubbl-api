using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Commands.DeleteTeamInvitation.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/invitations/{invitationId:ObjectId}/delete", Name = "DeleteTeamInvitation")]
    public class DeleteTeamInvitationController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public DeleteTeamInvitationController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), 204)]
        [SwaggerOperation(Tags = new[] {"Team Invitations"})]
        public async Task<IActionResult> DeleteTeamInvitation([FromRoute] string teamId,
            [FromRoute] string invitationId,
            CancellationToken cancellationToken)
        {
            var command = new DeleteTeamInvitationCommand
            (
                ObjectId.Parse(teamId),
                ObjectId.Parse(invitationId)
            );

            await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return StatusCode(204);
        }
    }
}