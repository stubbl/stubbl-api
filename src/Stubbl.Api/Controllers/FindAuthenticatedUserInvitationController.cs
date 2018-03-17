using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Queries.FindAuthenticatedUserInvitation.Version1;

namespace Stubbl.Api.Controllers
{
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
        public async Task<IActionResult> FindAuthenticatedUserInvitation([FromRoute] string invitationId,
            CancellationToken cancellationToken)
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