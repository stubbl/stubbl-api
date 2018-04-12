using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Filters;
using Gunnsoft.Cqs.Commands;
using Microsoft.AspNetCore.Mvc;
using Stubbl.Api.Commands.CreateAuthenticatedUser.Version1;
using Stubbl.Api.Models.CreateAuthenticatedUser.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("user/create", Name = "CreateAuthenticatedUser")]
    public class CreateAuthenticatedUserController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public CreateAuthenticatedUserController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), 204)]
        [SwaggerOperation(Tags = new[] {"Authenticated User"})]
        [ValidateModelState]
        public async Task<IActionResult> CreateAuthenticatedUser([FromBody] CreateAuthenticatedUserRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateAuthenticatedUserCommand
            (
                request.Name,
                request.EmailAddress
            );

            await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return StatusCode(204);
        }
    }
}