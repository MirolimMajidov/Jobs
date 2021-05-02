using AccountService.DataProvider;
using AccountService.Models;
using Jobs.SharedModel.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.SharedModel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AccountService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private readonly JobsContext _context;

        public AccountController(JobsContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("Authorization")]
        [SwaggerOperation(Summary = "For authorization user to jobs services")]
        [SwaggerResponse(200, "Return AuthInfo = {token, refreshToken} when authorization finished successfully", typeof(Tuple<string, string>))]
        [SwaggerResponse(404, "A user with the specified login and password was not found", typeof(RequestModel))]
        public async Task<RequestModel> Authorization([FromForm, SwaggerParameter("Login of user", Required = true)] string login, [FromForm, SwaggerParameter("Orginal password of user", Required = true)] string password)
        {
            if (login.IsNullOrEmpty() || password.IsNullOrEmpty())
                return await RequestModel.NotAccessAsync();

            var user = _context.Users.FirstOrDefault(u => u.Login == login && u.HashPassword == Encryptor.SH1Hash(password));
            if (user == null)
                return await RequestModel.NotAccessAsync();

            var (accessToken, refreshToken, hashToken) = await GenerateToken(user: user);
            user.Token = hashToken;
            user.RefreshToken = refreshToken;
            user.LastOnline = DateTime.Now;
            _context.Update(user);
            await _context.SaveChangesAsync();

            var authInfo = new { accessToken, refreshToken };
            return await RequestModel.SuccessAsync(authInfo);
        }

        [Authorize]
        [HttpPost("RefreshToken")]
        [SwaggerOperation(Summary = "For refreshing exist token of user")]
        [SwaggerResponse(200, "Return AuthInfo = {token, refreshToken} when token updated successfully", typeof(Tuple<string, string>))]
        [SwaggerResponse(404, "A user with the specified login and password was not found", typeof(NotFoundResult))]
        public async Task<RequestModel> RefreshToken([FromForm, SwaggerParameter("Last refresh token of user", Required = true)] string refreshToken)
        {
            if (refreshToken.IsNullOrEmpty())
                return await RequestModel.NotAccessAsync();

            var user = _context.Users.FirstOrDefault(u => u.RefreshToken == refreshToken && u.Token == Encryptor.SH1Hash(HttpContext.GetBearerToken()));
            if (user == null)
                return await RequestModel.NotFoundAsync();

            var (accessToken, refToken, hashToken) = await GenerateToken(user: user);
            user.Token = hashToken;
            user.RefreshToken = refToken;
            user.LastOnline = DateTime.Now;
            _context.Update(user);
            await _context.SaveChangesAsync();

            var authInfo = new { accessToken, refreshToken = refToken };

            return await RequestModel.SuccessAsync(authInfo);
        }

        [Authorize(Policy = "AllUsers")]
        [HttpPost("LogOut")]
        [SwaggerOperation(Summary = "To log out user from jobs services")]
        [SwaggerResponse(200, "If finished successfully", typeof(OkObjectResult))]
        public async Task<RequestModel> LogOut()
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == User.GetUserId());
            if (user == null)
                return await RequestModel.NotAccessAsync();

            user.Token = string.Empty;
            user.RefreshToken = string.Empty;
            user.LastOnline = DateTime.Now;
            _context.Update(user);
            await _context.SaveChangesAsync();

            return await RequestModel.SuccessAsync();
        }

        [HttpGet("UserInfo")]
        public async Task<RequestModel> UserInfo()
        {
            return await RequestModel.SuccessAsync(new { User.Identity?.IsAuthenticated, UserName = User.GetUserName(), UserId = User.GetUserId(), UserRole = User.GetUserRole(), UserRoleId = User.GetUserRoleId() });
        }

        /// <summary>
        /// This is for creating new token by new identity
        /// </summary>
        /// <param name="user">User info</param>
        /// <returns>Return new generated token, refresh token and hash token</returns>
        private static async Task<(string accessToken, string refreshToken, string hashToken)> GenerateToken(User user)
        {
            return await Task.Run(async () =>
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var newIdentity = await GenerateIdentity(user: user);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = AuthOptions.ISSUER,
                    Audience = AuthOptions.AUDIENCE,
                    Subject = newIdentity,
                    Expires = DateTime.Now.Add(TimeSpan.FromMinutes(AuthOptions.TokenLIFETIME)),
                    SigningCredentials = new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var accessToken = tokenHandler.WriteToken(token);
                var hashToken = Encryptor.SH1Hash(accessToken);
                var refreshToken = Guid.NewGuid().ToString();

                return (accessToken, refreshToken, hashToken);
            });
        }

        /// <summary>
        /// This is for creating new Identity by existent old identity or user and claims (IP address, mobile model, mobile id)
        /// </summary>
        /// <param name="user">User info</param>
        /// <returns>Return new created ClaimsIdentity</returns>
        private static async Task<ClaimsIdentity> GenerateIdentity(User user)
        {
            return await Task.Run(() =>
            {
                if (user == null) return null;

                List<Claim> claims = new();
                claims.Add(CreateClaim(ClaimTypes.Name, user.Name));
                claims.Add(CreateClaim("UserId", user.Id.ToString()));
                claims.Add(CreateClaim(ClaimTypes.Role, user.Role.GetDisplayName()));
                claims.Add(CreateClaim("RoleId", ((int)user.Role).ToString()));

                static Claim CreateClaim(string key, string value) => new(key, value);

                ClaimsIdentity claimsIdentity = new(claims, JwtBearerDefaults.AuthenticationScheme, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            });
        }
    }
}
