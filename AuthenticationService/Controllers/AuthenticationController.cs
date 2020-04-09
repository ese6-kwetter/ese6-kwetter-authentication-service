using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Models;
using AuthenticationService.Services;
using AuthenticationService.Views;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Controllers
{
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly 

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("google")]
        public async Task<IActionResult> Google([FromBody] UserView userView)
        {
            try
            {
                var payload = GoogleJsonWebSignature
                    .ValidateAsync(userView.TokenId, new GoogleJsonWebSignature.ValidationSettings()).Result;
                var user = await _authenticationService.Authenticate(payload);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,
                        Security.Encrypt(AppSettings.Settings.JwtEmailEncryption, user.Email)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppSettings.Settings.JwtSecret));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(string.Empty,
                    string.Empty,
                    claims,
                    expires: DateTime.Now.AddSeconds(55 * 60),
                    signingCredentials: credentials);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpGet("fill")]
        public async Task<IActionResult> Fill()
        {
            await _authenticationService.Fill();
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("users")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _authenticationService.Get());
        }
    }
}
