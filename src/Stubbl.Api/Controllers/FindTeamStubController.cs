using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Queries.FindTeamStub.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/stubs/{stubId:ObjectId}/find", Name = "FindTeamStub")]
    public class FindTeamStubController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public FindTeamStubController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(FindTeamStubProjection), 200)]
        [SwaggerOperation(Tags = new[] {"Team Stubs"})]
        public async Task<IActionResult> FindTeamStub([FromRoute] string teamId, [FromRoute] string stubId,
            CancellationToken cancellationToken)
        {
            var query = new FindTeamStubQuery
            (
                ObjectId.Parse(teamId),
                ObjectId.Parse(stubId)
            );

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection);
        }
    }
}