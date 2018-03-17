using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Queries.CountTeamRoles.Version1;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/roles/count", Name = "CountTeamRoles")]
    public class CountTeamRolesController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public CountTeamRolesController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CountTeamRolesProjection), 200)]
        public async Task<IActionResult> CountTeamRoles([FromRoute] string teamId, CancellationToken cancellationToken)
        {
            var query = new CountTeamRolesQuery
            (
                ObjectId.Parse(teamId)
            );

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection);
        }
    }
}