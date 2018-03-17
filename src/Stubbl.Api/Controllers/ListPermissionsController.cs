using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using Stubbl.Api.Queries.ListPermissions.Version1;

namespace Stubbl.Api.Controllers
{
    [ApiVersion("1")]
    [Route("permissions/list", Name = "ListPermissions")]
    public class ListPermissionsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ListPermissionsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListPermissionsProjection), 200)]
        public async Task<IActionResult> ListPermissions(CancellationToken cancellationToken)
        {
            var query = new ListPermissionsQuery();
            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection.Permissions);
        }
    }
}