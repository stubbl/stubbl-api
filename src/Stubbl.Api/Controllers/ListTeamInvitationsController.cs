namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using CodeContrib.Queries;
   using Core.Queries.ListTeamInvitations.Version1;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("teams/{teamId:ObjectId}/invitations/list", Name = "ListTeamInvitations")]
   public class ListTeamInvitationsController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public ListTeamInvitationsController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(ListTeamInvitationsProjection), 200)]
      public async Task<IActionResult> ListTeamInvitations([FromRoute] string teamId, CancellationToken cancellationToken)
      {
         var query = new ListTeamInvitationsQuery
         (
            ObjectId.Parse(teamId)
         );

         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection.Invitations);
      }
   }
}