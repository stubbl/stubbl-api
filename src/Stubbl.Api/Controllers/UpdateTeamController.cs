namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using CodeContrib.Commands;
   using Core.Commands.UpdateTeam.Version1;
   using Filters;
   using Microsoft.AspNetCore.Mvc;
   using Models.UpdateTeam.Version1;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("teams/{teamId:ObjectId}/update", Name = "UpdateTeam")]
   public class UpdateTeamController : Controller
   {
      private readonly ICommandDispatcher _commandDispatcher;

      public UpdateTeamController(ICommandDispatcher commandDispatcher)
      {
         _commandDispatcher = commandDispatcher;
      }

      [HttpPost]
      [ProducesResponseType(typeof(object), 204)]
      [ValidateModelState]
      public async Task<IActionResult> UpdateTeam([FromRoute] string teamId, [FromBody] UpdateTeamRequest request, CancellationToken cancellationToken)
      {
         var command = new UpdateTeamCommand
         (
            ObjectId.Parse(teamId),
            request.Name
         );

         await _commandDispatcher.DispatchAsync(command, cancellationToken);

         return StatusCode(204);
      }
   }
}