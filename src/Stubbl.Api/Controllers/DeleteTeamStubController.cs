namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using CodeContrib.Commands;
   using Core.Commands.DeleteTeamStub.Version1;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("teams/{teamId:ObjectId}/stubs/{stubId:ObjectId}/delete", Name = "DeleteTeamStub")]
   public class DeleteTeamStubController : Controller
   {
      private readonly ICommandDispatcher _commandDispatcher;

      public DeleteTeamStubController(ICommandDispatcher commandDispatcher)
      {
         _commandDispatcher = commandDispatcher;
      }

      [HttpPost]
      [ProducesResponseType(typeof(object), 204)]
      public async Task<IActionResult> DeleteTeamStub([FromRoute] string teamId, [FromRoute] string stubId,
         CancellationToken cancellationToken)
      {
         var command = new DeleteTeamStubCommand
         (
            ObjectId.Parse(teamId),
            ObjectId.Parse(stubId)
         );

         await _commandDispatcher.DispatchAsync(command, cancellationToken);

         return StatusCode(204);
      }
   }
}