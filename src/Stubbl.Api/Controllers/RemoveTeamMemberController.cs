using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Commands.RemoveTeamMember.Version1;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/members/{memberId:ObjectId}/remove", Name = "RemoveTeamMember")]
    public class RemoveTeamMemberController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public RemoveTeamMemberController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), 204)]
        public async Task<IActionResult> RemoveTeamMember([FromRoute] string teamId, [FromRoute] string memberId,
            CancellationToken cancellationToken)
        {
            var command = new RemoveTeamMemberCommand
            (
                ObjectId.Parse(teamId),
                ObjectId.Parse(memberId)
            );

            await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return StatusCode(204);
        }
    }
}