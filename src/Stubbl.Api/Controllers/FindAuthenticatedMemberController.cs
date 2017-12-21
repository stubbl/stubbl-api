namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.Queries;
   using Core.Queries.FindAuthenticatedMember.Version1;
   using Microsoft.AspNetCore.Mvc;

   [ApiVersion("1")]
   [Route("member/find", Name = "FindAuthenticatedMember")]
   public class FindAuthenticatedMemberController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public FindAuthenticatedMemberController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(FindAuthenticatedMemberProjection), 200)]
      public async Task<IActionResult> FindAuthenticatedMember(CancellationToken cancellationToken)
      {
         var query = new FindAuthenticatedMemberQuery();
         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection);
      }
    }
}
