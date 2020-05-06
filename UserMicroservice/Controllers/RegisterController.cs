using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserMicroservice.Models;
using UserMicroservice.Services;

namespace UserMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _service;

        public RegisterController(IRegisterService service)
        {
            _service = service;
        }

        [HttpPost("password")]
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

        [HttpPost("google")]
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

        [HttpPost("apple")]
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
