using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using Stubbl.Api.Queries.ListAuthenticatedUserInvitations.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

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
        [ProducesResponseType(typeof(IReadOnlyCollection<Invitation>), 200)]
        [SwaggerOperation(Tags = new[] { "Authenticated User Invitations" })]
        public async Task<IActionResult> ListAuthenticatedUserInvitations(CancellationToken cancellationToken)
        {
            var query = new ListAuthenticatedUserInvitationsQuery();
            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection.Invitations);
        }
    }
}