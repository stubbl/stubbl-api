namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using CodeContrib.Queries;
   using Core.Queries.ListTeamRoles.Version1;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("teams/{teamId:ObjectId}/roles/list", Name = "ListTeamRoles")]
   public class ListTeamRolesController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public ListTeamRolesController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(ListTeamRolesProjection), 200)]
      public async Task<IActionResult> ListTeamRoles([FromRoute] string teamId, CancellationToken cancellationToken)
      {
         var query = new ListTeamRolesQuery
         (
            ObjectId.Parse(teamId)
         );

         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection.Roles);
      }
   }
}