namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.Queries;
   using Core.Queries.ListTeams.Version1;
   using Microsoft.AspNetCore.Mvc;

   [ApiVersion("1")]
   [Route("teams/list", Name = "ListTeams")]
   public class ListTeamsController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public ListTeamsController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(ListTeamsProjection), 200)]
      public async Task<IActionResult> ListTeams(CancellationToken cancellationToken)
      {
         var query = new ListTeamsQuery();
         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection.Teams);
      }
   }
}