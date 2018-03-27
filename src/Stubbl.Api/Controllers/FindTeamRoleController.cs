using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Queries.FindTeamRole.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/roles/{roleId:ObjectId}/find", Name = "FindTeamRole")]
    public class FindTeamRoleController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public FindTeamRoleController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(FindTeamRoleProjection), 200)]
        [SwaggerOperation(Tags = new[] {"Team Roles"})]
        public async Task<IActionResult> FindTeamRole([FromRoute] string teamId, [FromRoute] string roleId,
            CancellationToken cancellationToken)
        {
            var query = new FindTeamRoleQuery
            (
                ObjectId.Parse(teamId),
                ObjectId.Parse(roleId)
            );

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection);
        }
    }
}