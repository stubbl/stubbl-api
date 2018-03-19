using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Queries.FindTeam.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/find", Name = "FindTeam")]
    public class FindTeamController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public FindTeamController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(FindTeamProjection), 200)]
        [SwaggerOperation(Tags = new[] { "Teams" })]
        public async Task<IActionResult> FindTeam([FromRoute] string teamId, CancellationToken cancellationToken)
        {
            var query = new FindTeamQuery
            (
                ObjectId.Parse(teamId)
            );

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection);
        }
    }
}