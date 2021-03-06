using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Queries.ListTeamStubs.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/stubs/list", Name = "ListTeamStubs")]
    public class ListTeamStubsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ListTeamStubsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<Stub>), 200)]
        [SwaggerOperation(Tags = new[] {"Team Stubs"})]
        public async Task<IActionResult> ListTeamStubs([FromRoute] string teamId, [FromQuery] string search,
            [FromQuery] int? pageNumber, [FromQuery] int? pageSize, CancellationToken cancellationToken)
        {
            var query = new ListTeamStubsQuery
            (
                ObjectId.Parse(teamId),
                search,
                pageNumber.GetValueOrDefault(1),
                pageSize.GetValueOrDefault(10)
            );

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            Response.Headers.Add("X-Paging-PageCount", projection.Paging.PageCount.ToString());
            Response.Headers.Add("X-Paging-PageNumber", projection.Paging.PageNumber.ToString());
            Response.Headers.Add("X-Paging-PageSize", projection.Paging.PageSize.ToString());
            Response.Headers.Add("X-Paging-TotalCount", projection.Paging.TotalCount.ToString());

            return StatusCode(200, projection.Stubs);
        }
    }
}