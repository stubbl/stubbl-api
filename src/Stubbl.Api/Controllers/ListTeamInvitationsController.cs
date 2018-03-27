using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Stubbl.Api.Queries.ListTeamInvitations.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("teams/{teamId:ObjectId}/invitations/list", Name = "ListTeamInvitations")]
    public class ListTeamInvitationsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ListTeamInvitationsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<Invitation>), 200)]
        [SwaggerOperation(Tags = new[] {"Team Invitations"})]
        public async Task<IActionResult> ListTeamInvitations([FromRoute] string teamId,
            CancellationToken cancellationToken)
        {
            var query = new ListTeamInvitationsQuery
            (
                ObjectId.Parse(teamId)
            );

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection.Invitations);
        }
    }
}