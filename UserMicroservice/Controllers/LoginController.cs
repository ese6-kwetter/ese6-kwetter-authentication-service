using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserMicroservice.Entities;
using UserMicroservice.Helpers;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserMicroservice.Exceptions;
using UserMicroservice.Models;
using UserMicroservice.Services;

namespace UserMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _service;

        public LoginController(ILoginService service)
        {
            _service = service;
        }

        [HttpPost("password")]
        public async Task<IActionResult> LoginPasswordAsync([FromBody] LoginModel model)
        {
            try
            {
                var user = await _service.LoginPasswordAsync(model.Email, model.Password);

                return Ok(user);
            }
            catch (EmailNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("google")]
        public async Task<IActionResult> LoginGoogleAsync([FromBody] LoginModel model)
        {
            try
            {
                var user = await _service.LoginGoogleAsync(model.TokenId);

                return Ok(user);
            }
            catch (GoogleAccountNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("apple")]
        public async Task<IActionResult> LoginAppleAsync([FromBody] LoginModel model)
        {
            try
            {
                var user = await _service.LoginAppleAsync(model.TokenId);

                return Ok(user);
            }
            catch (AppleAccountNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
