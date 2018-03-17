using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using Stubbl.Api.Queries.ListAuthenticatedUserInvitations.Version1;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("user/invitations/list", Name = "ListAuthenticatedUserInvitations")]
    public class ListAuthenticatedUserInvitationsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ListAuthenticatedUserInvitationsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListAuthenticatedUserInvitationsProjection), 200)]
        public async Task<IActionResult> ListAuthenticatedUserInvitations(CancellationToken cancellationToken)
        {
            var query = new ListAuthenticatedUserInvitationsQuery();
            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection.Invitations);
        }
    }
}