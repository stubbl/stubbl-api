namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using CodeContrib.Queries;
   using Core.Queries.CountTeamStubs.Version1;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("teams/{teamId:ObjectId}/stubs/count", Name = "CountTeamStubs")]
   public class CountTeamStubsController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public CountTeamStubsController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(CountTeamStubsProjection), 200)]
      public async Task<IActionResult> CountTeamStubs([FromRoute] string teamId, CancellationToken cancellationToken)
      {
         var query = new CountTeamStubsQuery
         (
            ObjectId.Parse(teamId)
         );

         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection);
      }
   }
}