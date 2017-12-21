namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.Queries;
   using Core.Queries.CountPermissions.Version1;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("permissions/count", Name = "CountPermissions")]
   public class CountPermissionsController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public CountPermissionsController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(CountPermissionsProjection), 200)]
      public async Task<IActionResult> CountPermissions(CancellationToken cancellationToken)
      {
         var query = new CountPermissionsQuery();

         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection);
      }
   }
}