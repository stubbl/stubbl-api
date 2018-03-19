using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Queries.FindTeamInvitation.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/invitation/{invitationId:ObjectId}/find", Name = "FindTeamInvitation")]
    public class FindTeamInvitationController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public FindTeamInvitationController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(FindTeamInvitationProjection), 200)]
        [SwaggerOperation(Tags = new[] { "Team Invitations" })]
        public async Task<IActionResult> FindTeamInvitation([FromRoute] string teamId, [FromRoute] string invitationId,
            CancellationToken cancellationToken)
        {
            var query = new FindTeamInvitationQuery
            (
                ObjectId.Parse(teamId),
                ObjectId.Parse(invitationId)
            );

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection);
        }
    }
}