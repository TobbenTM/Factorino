using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace FNO.WebApp.Controllers.Api
{
    [ApiController]
    [Route("api/xsrf")]
    public class XsrfTokenController : Controller
    {
        private readonly IAntiforgery _antiforgery;

        public XsrfTokenController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);

            return new ObjectResult(new
            {
                Token = tokens.RequestToken,
                TokenName = tokens.HeaderName,
            });
        }
    }
}
