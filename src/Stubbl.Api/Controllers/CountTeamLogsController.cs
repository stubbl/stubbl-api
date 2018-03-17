namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Gunnsoft.Cqs.Queries;
   using Core.Queries.CountTeamLogs.Version1;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("teams/{teamId:ObjectId}/logs/count", Name = "CountTeamLogs")]
   public class CountTeamLogsController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public CountTeamLogsController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(CountTeamLogsProjection), 200)]
      public async Task<IActionResult> CountTeamLogs([FromRoute] string teamId, CancellationToken cancellationToken)
      {
         var query = new CountTeamLogsQuery
         (
            ObjectId.Parse(teamId)
         );

         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection);
      }
   }
}