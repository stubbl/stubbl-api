using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using Stubbl.Api.Queries.CountPermissions.Version1;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("permissions/count", Name = "CountPermissions")]
    public class CountPermissionsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public CountPermissionsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CountPermissionsProjection), 200)]
        public async Task<IActionResult> CountPermissions(CancellationToken cancellationToken)
        {
            var query = new CountPermissionsQuery();

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection);
        }
    }
}