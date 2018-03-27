using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Queries.CountTeamStubs.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/stubs/count", Name = "CountTeamStubs")]
    public class CountTeamStubsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public CountTeamStubsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CountTeamStubsProjection), 200)]
        [SwaggerOperation(Tags = new[] {"Team Stubs"})]
        public async Task<IActionResult> CountTeamStubs([FromRoute] string teamId, CancellationToken cancellationToken)
        {
            var query = new CountTeamStubsQuery
            (
                ObjectId.Parse(teamId)
            );

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection);
        }
    }
}