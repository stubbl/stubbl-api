using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Queries.FindTeamLog.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/logs/{logId:ObjectId}/find", Name = "FindTeamLog")]
    public class FindTeamLogController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public FindTeamLogController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(FindTeamLogProjection), 200)]
        [SwaggerOperation(Tags = new[] {"Team Logs"})]
        public async Task<IActionResult> FindTeamLog([FromRoute] string teamId, [FromRoute] string logId,
            CancellationToken cancellationToken)
        {
            var query = new FindTeamLogQuery
            (
                ObjectId.Parse(teamId),
                ObjectId.Parse(logId)
            );

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection);
        }
    }
}