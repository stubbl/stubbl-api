using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stubbl.Api.Options;
using Stubbl.Api.Queries.Canary.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1")]
    [Route("canary", Name = "Canary")]
    public class CanaryController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly StubblApiOptions _stubblApiOptions;

        public CanaryController(IQueryDispatcher queryDispatcher,
            IOptions<StubblApiOptions> stubblApiOptions)
        {
            _queryDispatcher = queryDispatcher;
            _stubblApiOptions = stubblApiOptions.Value;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CanaryProjection), 200)]
        [SwaggerOperation(Tags = new[] {"Health"})]
        public async Task<IActionResult> Canary([FromQuery] string secret, CancellationToken cancellationToken)
        {
            if (_stubblApiOptions.ApiKey == null
                || secret != _stubblApiOptions.ApiKey)
            {
                return NotFound();
            }

            var query = new CanaryQuery();
            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection);
        }
    }
}