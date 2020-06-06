using Microsoft.AspNetCore.Mvc;

namespace VoiceChatAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        public HealthCheckController()
        {
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
