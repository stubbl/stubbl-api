namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Gunnsoft.Cqs.Queries;
   using Core.Queries.CountTeamInvitations.Version1;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("teams/{teamId:ObjectId}/invitations/count", Name = "CountTeamInvitations")]
   public class CountTeamInvitationsController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public CountTeamInvitationsController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(CountTeamInvitationsProjection), 200)]
      public async Task<IActionResult> CountTeamInvitations([FromRoute] string teamId, CancellationToken cancellationToken)
      {
         var query = new CountTeamInvitationsQuery
         (
            ObjectId.Parse(teamId)
         );

         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection);
      }
   }
}