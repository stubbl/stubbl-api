namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.Commands;
   using Core.Commands.DeleteTeamInvitation.Version1;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

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
      public async Task<IActionResult> DeleteTeamInvitation([FromRoute] string teamId, [FromRoute] string invitationId,
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