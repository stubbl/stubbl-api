using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Queries.ListTeamRoles.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/roles/list", Name = "ListTeamRoles")]
    public class ListTeamRolesController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ListTeamRolesController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<Role>), 200)]
        [SwaggerOperation(Tags = new[] { "Team Roles" })]
        public async Task<IActionResult> ListTeamRoles([FromRoute] string teamId, CancellationToken cancellationToken)
        {
            var query = new ListTeamRolesQuery
            (
                ObjectId.Parse(teamId)
            );

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection.Roles);
        }
    }
}