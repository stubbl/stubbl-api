namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.Commands;
   using Core.Commands.ResendTeamInvitation.Version1;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("teams/{teamId:ObjectId}/invitations/{invitationId:ObjectId}/resend", Name = "ResendTeamInvitation")]
   public class ResendTeamInvitationController : Controller
   {
      private readonly ICommandDispatcher _commandDispatcher;

      public ResendTeamInvitationController(ICommandDispatcher commandDispatcher)
      {
         _commandDispatcher = commandDispatcher;
      }

      [HttpPost]
      [ProducesResponseType(typeof(object), 204)]
      public async Task<IActionResult> ResendTeamInvitation([FromRoute] string teamId, [FromRoute] string invitationId,
         CancellationToken cancellationToken)
      {
         var command = new ResendTeamInvitationCommand
         (
            ObjectId.Parse(teamId),
            ObjectId.Parse(invitationId)
         );

         await _commandDispatcher.DispatchAsync(command, cancellationToken);

         return StatusCode(204);
      }
   }
}