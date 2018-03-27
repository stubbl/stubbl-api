using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Queries.FindTeamMember.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/members/{memberId:ObjectId}/find", Name = "FindTeamMember")]
    public class FindTeamMemberController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public FindTeamMemberController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(FindTeamMemberProjection), 200)]
        [SwaggerOperation(Tags = new[] {"Team Members"})]
        public async Task<IActionResult> FindTeamMember([FromRoute] string teamId, [FromRoute] string memberId,
            CancellationToken cancellationToken)
        {
            var query = new FindTeamMemberQuery
            (
                ObjectId.Parse(teamId),
                ObjectId.Parse(memberId)
            );

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection);
        }
    }
}