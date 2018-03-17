namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Gunnsoft.Cqs.Queries;
   using Core.Queries.CountTeamMembers.Version1;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("teams/{teamId:ObjectId}/members/count", Name = "CountTeamMembers")]
   public class CountTeamMembersController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public CountTeamMembersController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(CountTeamMembersProjection), 200)]
      public async Task<IActionResult> CountTeamMembers([FromRoute] string teamId, CancellationToken cancellationToken)
      {
         var query = new CountTeamMembersQuery
         (
            ObjectId.Parse(teamId)
         );

         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection);
      }
   }
}