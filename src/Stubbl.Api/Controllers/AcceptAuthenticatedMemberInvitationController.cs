namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.Commands;
   using Core.Commands.AcceptAuthenticatedMemberInvitation.Version1;
   using Filters;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("member/invitations/{invitationId:ObjectId}/accept", Name = "AcceptTeamInvitation")]
   public class AcceptAuthenticatedMemberInvitationController : Controller
   {
      private readonly ICommandDispatcher _commandDispatcher;

      public AcceptAuthenticatedMemberInvitationController(ICommandDispatcher commandDispatcher)
      {
         _commandDispatcher = commandDispatcher;
      }

      [HttpPost]
      [ProducesResponseType(typeof(object), 204)]
      [ValidateModelState]
      public async Task<IActionResult> AcceptAuthenticatedMemberInvitation([FromRoute] string invitationId,
         CancellationToken cancellationToken)
      {
         var command = new AcceptAuthenticatedMemberInvitationCommand
         (
            ObjectId.Parse(invitationId)
         );

         await _commandDispatcher.DispatchAsync(command, cancellationToken);

         return StatusCode(204);
      }
   }
}