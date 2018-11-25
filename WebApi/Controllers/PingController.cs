namespace WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        // [GenerateAntiforgeryTokenCookieForAjax]
        public IActionResult Index() => Ok("Ping");
    }
}