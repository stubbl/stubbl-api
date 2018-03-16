namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using CodeContrib.Queries;
   using Core.Queries.CountAuthenticatedUserInvitations.Version1;
   using Microsoft.AspNetCore.Mvc;

   [ApiVersion("1")]
   [Route("user/invitations/count", Name = "CountAuthenticatedUserInvitations")]
   public class CountAuthenticatedUserInvitationsController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public CountAuthenticatedUserInvitationsController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(CountAuthenticatedUserInvitationsProjection), 200)]
      public async Task<IActionResult> CountAuthenticatedUserInvitiations(CancellationToken cancellationToken)
      {
         var query = new CountAuthenticatedUserInvitationsQuery();

         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection);
      }
   }
}