using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using Stubbl.Api.Queries.CountAuthenticatedUserTeamInvitations.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("user/team-invitations/count", Name = "CountAuthenticatedUserTeamInvitations")]
    public class CountAuthenticatedUserTeamInvitationsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public CountAuthenticatedUserTeamInvitationsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CountAuthenticatedUserTeamInvitationsProjection), 200)]
        [SwaggerOperation(Tags = new[] {"Authenticated User Team Invitations"})]
        public async Task<IActionResult> CountAuthenticatedUserTeamInvitiations(CancellationToken cancellationToken)
        {
            var query = new CountAuthenticatedUserTeamInvitationsQuery();

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection);
        }
    }
}