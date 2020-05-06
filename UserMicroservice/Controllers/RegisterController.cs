using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserMicroservice.Models;
using UserMicroservice.Services;

namespace UserMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _service;

        public RegisterController(IRegisterService service)
        {
            _service = service;
        }
        
        /// <summary>
        ///     Register a User with password.
        /// </summary>
        /// <param name="model">Registration credentials</param>
        /// <returns>User without sensible data</returns>
        [HttpPost("password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterPasswordAsync([FromBody] RegisterModel model)
        {
            try
            {
                var user = await _service.RegisterPasswordAsync(model.Username, model.Email, model.Password);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        /// <summary>
        ///     Register a User with a Google account.
        /// </summary>
        /// <param name="model">Registration credentials</param>
        /// <returns>User without sensible data</returns>
        [HttpPost("google")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterGoogleAsync([FromBody] RegisterModel model)
        {
            try
            {
                var user = await _service.RegisterGoogleAsync(model.TokenId);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///     Register a User with a Apple account.
        /// </summary>
        /// <param name="model">Registration credentials</param>
        /// <returns>User without sensible data</returns>
        [HttpPost("apple")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAppleAsync([FromBody] RegisterModel model)
        {
            try
            {
                var user = await _service.RegisterAppleAsync(model.TokenId);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
