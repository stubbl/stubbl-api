using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using Stubbl.Api.Queries.ListTeams.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/list", Name = "ListTeams")]
    public class ListTeamsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ListTeamsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<Team>), 200)]
        [SwaggerOperation(Tags = new[] { "Teams" })]
        public async Task<IActionResult> ListTeams(CancellationToken cancellationToken)
        {
            var query = new ListTeamsQuery();
            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection.Teams);
        }
    }
}