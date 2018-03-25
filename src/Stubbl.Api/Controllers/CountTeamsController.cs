using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using Stubbl.Api.Queries.CountTeams.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

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
        [SwaggerOperation(Tags = new[] { "Teams" })]
        public async Task<IActionResult> CountTeams(CancellationToken cancellationToken)
        {
            var query = new CountTeamsQuery();

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection);
        }
    }
}