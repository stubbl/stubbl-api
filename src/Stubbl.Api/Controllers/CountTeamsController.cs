using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using Stubbl.Api.Queries.CountTeams.Version1;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/count", Name = "CountTeams")]
    public class CountTeamsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public CountTeamsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CountTeamsProjection), 200)]
        public async Task<IActionResult> CountTeams([FromRoute] string teamId, CancellationToken cancellationToken)
        {
            var query = new CountTeamsQuery();

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection);
        }
    }
}