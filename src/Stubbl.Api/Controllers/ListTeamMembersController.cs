using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Queries.ListTeamMembers.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/members/list", Name = "ListTeamMembers")]
    public class ListTeamMembersController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ListTeamMembersController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<Member>), 200)]
        [SwaggerOperation(Tags = new[] { "Team Members" })]
        public async Task<IActionResult> ListTeamMembers([FromRoute] string teamId, [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize, CancellationToken cancellationToken)
        {
            var query = new ListTeamMembersQuery
            (
                ObjectId.Parse(teamId),
                pageNumber.GetValueOrDefault(1),
                pageSize.GetValueOrDefault(10)
            );

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            Response.Headers.Add("X-Paging-PageCount", projection.Paging.PageCount.ToString());
            Response.Headers.Add("X-Paging-PageNumber", projection.Paging.PageNumber.ToString());
            Response.Headers.Add("X-Paging-PageSize", projection.Paging.PageSize.ToString());
            Response.Headers.Add("X-Paging-TotalCount", projection.Paging.TotalCount.ToString());

            return StatusCode(200, projection.Members);
        }
    }
}