using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Services;

namespace UserService.Controllers
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
        public async Task<IActionResult> RegisterPassword([FromBody] RegisterModel model)
        {
            var user = await _service.RegisterPassword(model.Username, model.Email, model.Password);

            if (user == null)
                return BadRequest();

            return Ok(user);
        }

        [HttpPost("google")]
        public async Task<IActionResult> RegisterGoogle([FromBody] RegisterModel model)
        {
            var user = await _service.RegisterGoogle(model.TokenId);

            if (user == null)
                return BadRequest();

            return Ok(user);
        }

        [HttpPost("apple")]
        public async Task<IActionResult> RegisterApple([FromBody] RegisterModel model)
        {
            var user = await _service.RegisterApple(model.TokenId);

            if (user == null)
                return BadRequest();

            return Ok(user);
        }
    }
}
