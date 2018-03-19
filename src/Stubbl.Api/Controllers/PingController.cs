using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stubbl.Api.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1")]
    [Route("ping", Name = "Ping")]
    public class PingController : Controller
    {
        private readonly StubblApiOptions _stubblApiOptions;

        public PingController(IOptions<StubblApiOptions> stubblApiOptions)
        {
            _stubblApiOptions = stubblApiOptions.Value;
        }

        [HttpGet]
        [ProducesResponseType(typeof(object), 200)]
        [SwaggerOperation(Tags = new[] { "Health" })]
        public IActionResult Ping([FromQuery] string secret)
        {
            if (_stubblApiOptions.ApiKey == null
                || secret != _stubblApiOptions.ApiKey)
            {
                return NotFound();
            }

            return StatusCode(200);
        }
    }
}