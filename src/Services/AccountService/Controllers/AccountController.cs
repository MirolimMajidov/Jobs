using AccountService.Models;
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
    [Route("api/auth")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("Auth")]
        [SwaggerOperation(Summary = "Gets verification code to authorize user to the system")]
        [SwaggerResponse(0, "Return AuthInfo = {accessToken, refreshToken} when registration finished successfully", typeof(Tuple<string, string>))]
        public async Task<IActionResult> Authorization()
        {
            var (accessToken, refreshToken, hashToken) = await GenerateToken(user: new User() { Name = "Test" });
            var authInfo = new { accessToken, refreshToken };
            return new OkObjectResult(authInfo);
        }

        [Authorize]
        [HttpGet("UserInfo")]
        public IActionResult UserInfo()
        {
            return new OkObjectResult(new { User.Identity?.IsAuthenticated, User.Identity?.Name, UserId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value });
        }

        /// <summary>
        /// This is for creating new token by new identity
        /// </summary>
        /// <param name="user">User info</param>
        /// <param name="claims">Claims (IP address, mobile model, mobile id)</param>
        /// <returns>Return new generated token, refresh token and hash token</returns>
        private static async Task<(string accessToken, string refreshToken, string hashToken)> GenerateToken(User user, params (string type, string value)[] claims)
        {
            return await Task.Run(async () =>
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var newIdentity = await GenerateIdentity(user: user, paramClaims: claims);

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
        /// <param name="existentIdentity">User's old identity</param>
        /// <param name="user">User info</param>
        /// <param name="paramClaims">Claims (IP address, mobile model, mobile id)</param>
        /// <returns>Return new created ClaimsIdentity</returns>
        private static async Task<ClaimsIdentity> GenerateIdentity(User user, params (string type, string value)[] paramClaims)
        {
            return await Task.Run(() =>
            {
                if (user == null) return null;

                List<Claim> claims = new();
                claims.Add(CreateClaim(ClaimTypes.Name, user.Name));
                claims.Add(CreateClaim("UserId", user.Id.ToString()));
                claims.Add(CreateClaim(ClaimTypes.Role, "Admin"));

                foreach (var (type, value) in paramClaims)
                    claims.Add(CreateClaim(type, value));

                static Claim CreateClaim(string key, string value) => new(key, value);

                ClaimsIdentity claimsIdentity = new(claims, JwtBearerDefaults.AuthenticationScheme, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            });
        }
    }
}
