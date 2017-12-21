namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.Queries;
   using Core.Queries.FindAuthenticatedMemberInvitation.Version1;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("member/invitations/{invitationId:ObjectId}/find", Name = "FindAuthenticatedMemberInvitation")]
   public class FindAuthenticatedMemberInvitationController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public FindAuthenticatedMemberInvitationController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(FindAuthenticatedMemberInvitationProjection), 200)]
      public async Task<IActionResult> FindAuthenticatedMemberInvitation([FromRoute] string invitationId, CancellationToken cancellationToken)
      {
         var query = new FindAuthenticatedMemberInvitationQuery
         (
            ObjectId.Parse(invitationId)
         );
         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection);
      }
   }
}