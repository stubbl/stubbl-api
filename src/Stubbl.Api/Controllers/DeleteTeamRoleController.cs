namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Gunnsoft.Cqs.Commands;
   using Core.Commands.DeleteTeamRole.Version1;
   using Filters;
   using Microsoft.AspNetCore.Mvc;
   using Models.CreateTeamRole.Version1;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("teams/{teamId:ObjectId}/roles/{roleId:ObjectId}/delete", Name = "DeleteTeamRole")]
   public class DeleteTeamRoleController : Controller
   {
      private readonly ICommandDispatcher _commandDispatcher;

      public DeleteTeamRoleController(ICommandDispatcher commandDispatcher)
      {
         _commandDispatcher = commandDispatcher;
      }

      [HttpPost]
      [ProducesResponseType(typeof(CreateTeamRoleResponse), 201)]
      [ValidateModelState]
      public async Task<IActionResult> DeleteTeamRole([FromRoute] string teamId, [FromRoute] string roleId,
         CancellationToken cancellationToken)
      {
         var command = new DeleteTeamRoleCommand
         (
            ObjectId.Parse(teamId),
            ObjectId.Parse(roleId)
         );

         await _commandDispatcher.DispatchAsync(command, cancellationToken);

         return StatusCode(204);
      }
   }
}