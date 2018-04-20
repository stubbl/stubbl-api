using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using Stubbl.Api.Queries.ListAuthenticatedUserTeamInvitations.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("user/team-invitations/list", Name = "ListAuthenticatedUserTeamInvitations")]
    public class ListAuthenticatedUserTeamInvitationsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ListAuthenticatedUserTeamInvitationsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<Invitation>), 200)]
        [SwaggerOperation(Tags = new[] {"Authenticated User Team Invitations"})]
        public async Task<IActionResult> ListAuthenticatedUserTeamInvitations(CancellationToken cancellationToken)
        {
            var query = new ListAuthenticatedUserTeamInvitationsQuery();
            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection.Invitations);
        }
    }
}