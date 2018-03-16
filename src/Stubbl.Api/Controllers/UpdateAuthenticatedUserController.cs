namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using CodeContrib.Commands;
   using Core.Commands.UpdateAuthenticatedUser.Version1;
   using Filters;
   using Microsoft.AspNetCore.Mvc;
   using Models.UpdateAuthenticatedUser.Version1;

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
      public async Task<IActionResult> UpdateAuthenticatedUser([FromBody] UpdateAuthenticatedUserRequest request, CancellationToken cancellationToken)
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