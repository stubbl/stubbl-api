namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.Commands;
   using Core.Commands.DeclineAuthenticatedMemberInvitation.Version1;
   using Filters;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("invitations/{invitationId:ObjectId}/decline", Name = "DeclineTeamInvitation")]
   public class DeclineAuthenticatedMemberInvitationController : Controller
   {
      private readonly ICommandDispatcher _commandDispatcher;

      public DeclineAuthenticatedMemberInvitationController(ICommandDispatcher commandDispatcher)
      {
         _commandDispatcher = commandDispatcher;
      }

      [HttpPost]
      [ProducesResponseType(typeof(object), 204)]
      [ValidateModelState]
      public async Task<IActionResult> DeclineAuthenticatedMemberInvitation([FromRoute] string invitationId,
         CancellationToken cancellationToken)
      {
         var command = new DeclineAuthenticatedMemberInvitationCommand
         (
            ObjectId.Parse(invitationId)
         );

         await _commandDispatcher.DispatchAsync(command, cancellationToken);

         return StatusCode(204);
      }
   }
}