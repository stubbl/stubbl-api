namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.Queries;
   using Core.Queries.CountAuthenticatedMemberInvitations.Version1;
   using Microsoft.AspNetCore.Mvc;

   [ApiVersion("1")]
   [Route("member/invitations/count", Name = "CountAuthenticatedMemberInvitations")]
   public class CountAuthenticatedMemberInvitationsController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public CountAuthenticatedMemberInvitationsController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(CountAuthenticatedMemberInvitationsProjection), 200)]
      public async Task<IActionResult> CountAuthenticatedMemberInvitiations(CancellationToken cancellationToken)
      {
         var query = new CountAuthenticatedMemberInvitationsQuery();

         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection);
      }
   }
}