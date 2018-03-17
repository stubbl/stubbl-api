namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Gunnsoft.Cqs.Queries;
   using Core.Queries.FindAuthenticatedUserInvitation.Version1;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("user/invitations/{invitationId:ObjectId}/find", Name = "FindAuthenticatedUserInvitation")]
   public class FindAuthenticatedUserInvitationController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public FindAuthenticatedUserInvitationController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(FindAuthenticatedUserInvitationProjection), 200)]
      public async Task<IActionResult> FindAuthenticatedUserInvitation([FromRoute] string invitationId, CancellationToken cancellationToken)
      {
         var query = new FindAuthenticatedUserInvitationQuery
         (
            ObjectId.Parse(invitationId)
         );
         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection);
      }
   }
}