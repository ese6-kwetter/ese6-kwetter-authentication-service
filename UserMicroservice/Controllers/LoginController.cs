using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserMicroservice.Entities;
using UserMicroservice.Helpers;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using UserMicroservice.Exceptions;
using UserMicroservice.Models;
using UserMicroservice.Services;

namespace UserMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _service;

        public LoginController(ILoginService service)
        {
            _service = service;
        }
        
        /// <summary>
        ///     Login a User with password.
        /// </summary>
        /// <param name="model">Login credentials</param>
        /// <returns>User without sensible data</returns>
        [HttpPost("password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        ///     Login a User with a connected Google account.
        /// </summary>
        /// <param name="model">Login credentials</param>
        /// <returns>User without sensible data</returns>
        [HttpPost("google")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        ///     Login a User with a connected Apple account.
        /// </summary>
        /// <param name="model">Login credentials</param>
        /// <returns>User without sensible data</returns>
        [HttpPost("apple")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
