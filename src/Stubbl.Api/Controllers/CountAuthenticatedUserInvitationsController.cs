using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using Stubbl.Api.Queries.CountAuthenticatedUserInvitations.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
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
        [SwaggerOperation(Tags = new[] {"Authenticated User Invitations"})]
        public async Task<IActionResult> CountAuthenticatedUserInvitiations(CancellationToken cancellationToken)
        {
            var query = new CountAuthenticatedUserInvitationsQuery();

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection);
        }
    }
}