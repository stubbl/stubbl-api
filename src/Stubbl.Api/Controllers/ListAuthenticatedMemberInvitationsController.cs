namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.Queries;
   using Core.Queries.ListAuthenticatedMemberInvitations.Version1;
   using Microsoft.AspNetCore.Mvc;

   [ApiVersion("1")]
   [Route("member/invitations/list", Name = "ListAuthenticatedMemberInvitations")]
   public class ListAuthenticatedMemberInvitationsController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public ListAuthenticatedMemberInvitationsController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(ListAuthenticatedMemberInvitationsProjection), 200)]
      public async Task<IActionResult> ListAuthenticatedMemberInvitations(CancellationToken cancellationToken)
      {
         var query = new ListAuthenticatedMemberInvitationsQuery();
         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection);
      }
   }
}