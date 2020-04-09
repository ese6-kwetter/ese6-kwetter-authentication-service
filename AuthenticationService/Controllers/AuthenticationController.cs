using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Entities;
using AuthenticationService.Helpers;
using AuthenticationService.Services;
using AuthenticationService.Views;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _service;

        public AuthenticationController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("password")]
        public async Task<IActionResult> Password([FromBody] UserModel model)
        {
            var user = await _service.Password(model.Username, model.Password);

            if (user == null)
                return BadRequest();

            return Ok(user);
        }
        
        [HttpPost("google")]
        public async Task<IActionResult> Google([FromBody] UserModel model)
        {
            var user = await _service.Google();

            if (user == null)
                return BadRequest();

            return Ok(user);
        }
    }
}
