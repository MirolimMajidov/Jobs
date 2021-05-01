using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace JobService.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        [Authorize]
        [HttpGet("UserInfo")]
        public IActionResult UserInfo()
        {
            return new OkObjectResult(new { User.Identity?.IsAuthenticated, User.Identity?.Name, UserId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value });
        }
    }
}
