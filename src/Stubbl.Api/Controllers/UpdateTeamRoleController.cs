namespace Stubbl.Api.Controllers
{
   using System;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using CodeContrib.Commands;
   using Core.Commands.Shared.Version1;
   using Core.Commands.UpdateTeamRole.Version1;
   using Filters;
   using Microsoft.AspNetCore.Mvc;
   using Models.UpdateTeamRole.Version1;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("teams/{teamId:ObjectId}/roles/{roleId:ObjectId}/update", Name = "UpdateTeamRole")]
   public class UpdateTeamRoleController : Controller
   {
      private readonly ICommandDispatcher _commandDispatcher;

      public UpdateTeamRoleController(ICommandDispatcher commandDispatcher)
      {
         _commandDispatcher = commandDispatcher;
      }

      [HttpPost]
      [ProducesResponseType(typeof(object), 204)]
      [ValidateModelState]
      public async Task<IActionResult> UpdateTeamRole([FromRoute] string teamId, [FromRoute] string roleId, [FromBody] UpdateTeamRoleRequest request, CancellationToken cancellationToken)
      {
         var command = new UpdateTeamRoleCommand
         (
            ObjectId.Parse(teamId),
            ObjectId.Parse(roleId),
            request.Name,
            request.Permissions.Select(p => (Permission) Enum.Parse(typeof(Permission), p)).ToList()
         );

         await _commandDispatcher.DispatchAsync(command, cancellationToken);

         return StatusCode(204);
      }
   }
}