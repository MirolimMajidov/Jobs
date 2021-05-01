using Jobs.SharedModel.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return new OkObjectResult(new { User.Identity?.IsAuthenticated, UserName = User.GetUserName(), UserId = User.GetUserId(), UserRole = User.GetUserRole(), UserRoleId = User.GetUserRoleId() });
        }
    }
}
