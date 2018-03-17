using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Stubbl.Api.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1")]
    [Route("ping", Name = "Ping")]
    public class PingController : Controller
    {
        [HttpGet]
        [ProducesResponseType(typeof(object), 200)]
        public IActionResult Ping()
        {
            return StatusCode(200);
        }
    }
}