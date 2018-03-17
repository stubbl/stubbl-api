using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stubbl.Api.Queries.Canary.Version1;

namespace Stubbl.Api.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1")]
    [Route("canary", Name = "Canary")]
    public class CanaryController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public CanaryController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CanaryProjection), 200)]
        public async Task<IActionResult> Canary(CancellationToken cancellationToken)
        {
            var query = new CanaryQuery();
            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection);
        }
    }
}