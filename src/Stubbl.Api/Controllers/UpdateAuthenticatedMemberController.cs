namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.Commands;
   using Core.Commands.UpdateAuthenticatedMember.Version1;
   using Filters;
   using Microsoft.AspNetCore.Mvc;
   using Models.UpdateAuthenticatedMember.Version1;

   [ApiVersion("1")]
   [Route("member/update", Name = "UpdateAuthenticatedMember")]
   public class UpdateAuthenticatedMemberController : Controller
   {
      private readonly ICommandDispatcher _commandDispatcher;

      public UpdateAuthenticatedMemberController(ICommandDispatcher commandDispatcher)
      {
         _commandDispatcher = commandDispatcher;
      }

      [HttpPost]
      [ProducesResponseType(typeof(object), 204)]
      [ValidateModelState]
      public async Task<IActionResult> UpdateAuthenticatedMember([FromBody] UpdateAuthenticatedMemberRequest request, CancellationToken cancellationToken)
      {
         var command = new UpdateAuthenticatedMemberCommand
         (
            request.Name,
            request.EmailAddress
         );

         await _commandDispatcher.DispatchAsync(command, cancellationToken);

         return StatusCode(204);
      }
   }
}