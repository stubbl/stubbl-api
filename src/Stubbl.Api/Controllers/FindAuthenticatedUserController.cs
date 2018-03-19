using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using Stubbl.Api.Queries.FindAuthenticatedUser.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("member/find", Name = "FindAuthenticatedUser")]
    public class FindAuthenticatedUserController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public FindAuthenticatedUserController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(FindAuthenticatedUserProjection), 200)]
        [SwaggerOperation(Tags = new[] { "Authenticated User" })]
        public async Task<IActionResult> FindAuthenticatedUser(CancellationToken cancellationToken)
        {
            var query = new FindAuthenticatedUserQuery();
            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection);
        }
    }
}