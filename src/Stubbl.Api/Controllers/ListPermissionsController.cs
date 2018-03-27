using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Mvc;
using Stubbl.Api.Queries.ListPermissions.Version1;
using Stubbl.Api.Queries.Shared.Version1;
using Swashbuckle.AspNetCore.SwaggerGen;

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
        [ProducesResponseType(typeof(IReadOnlyCollection<Permission>), 200)]
        [SwaggerOperation(Tags = new[] {"Permissions"})]
        public async Task<IActionResult> ListPermissions(CancellationToken cancellationToken)
        {
            var query = new ListPermissionsQuery();
            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return StatusCode(200, projection.Permissions);
        }
    }
}