using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Filters;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using Stubbl.Api.Commands.UpdateAuthenticatedUser.Version1;
using Stubbl.Api.Models.UpdateAuthenticatedUser.Version1;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("user/update", Name = "UpdateAuthenticatedUser")]
    public class UpdateAuthenticatedUserController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public UpdateAuthenticatedUserController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), 204)]
        [ValidateModelState]
        public async Task<IActionResult> UpdateAuthenticatedUser([FromBody] UpdateAuthenticatedUserRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateAuthenticatedUserCommand
            (
                request.Name,
                request.EmailAddress
            );

            await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return StatusCode(204);
        }
    }
}