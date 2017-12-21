namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.Commands;
   using Core.Commands.CreateTeamInvitation.Version1;
   using Filters;
   using Microsoft.AspNetCore.Mvc;
   using Models.CreateTeamInvitation.Version1;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("teams/{teamId:ObjectId}/invitations/create", Name = "CreateTeamInvitation")]
   public class CreateTeamInvitationController : Controller
   {
      private readonly ICommandDispatcher _commandDispatcher;

      public CreateTeamInvitationController(ICommandDispatcher commandDispatcher)
      {
         _commandDispatcher = commandDispatcher;
      }

      [HttpPost]
      [ProducesResponseType(typeof(object), 204)]
      [ValidateModelState]
      public async Task<IActionResult> CreateTeamInvitation([FromRoute] string teamId, [FromRoute] string roleId, [FromBody] CreateTeamInvitationRequest request, CancellationToken cancellationToken)
      {
         var command = new CreateTeamInvitationCommand
         (
            ObjectId.Parse(teamId),
            ObjectId.Parse(roleId),
            request.EmailAddress
         );

         var @event = await _commandDispatcher.DispatchAsync(command, cancellationToken);

         var location = Url.RouteUrl("FindTeamInvitation", new { teamId = teamId, inviationId = @event.InvitationId }, null, Request.Host.Value);

         Response.Headers["Location"] = location;

         var response = new CreateTeamInvitationResponse(@event.InvitationId.ToString());

         return StatusCode(201, response);
      }
   }
}