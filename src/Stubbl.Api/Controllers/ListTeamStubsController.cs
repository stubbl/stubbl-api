namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.Queries;
   using Core.Queries.ListTeamStubs.Version1;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("teams/{teamId:ObjectId}/stubs/list", Name = "ListTeamStubs")]
   public class ListTeamStubsController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public ListTeamStubsController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(ListTeamStubsProjection), 200)]
      public async Task<IActionResult> ListTeamStubs([FromRoute] string teamId, [FromQuery] string search,
         [FromQuery] int? pageNumber, [FromQuery] int? pageSize, CancellationToken cancellationToken)
      {
         var query = new ListTeamStubsQuery
         (
            ObjectId.Parse(teamId),
            search,
            pageNumber.GetValueOrDefault(1),
            pageSize.GetValueOrDefault(10)
         );

         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection);
      }
   }
}