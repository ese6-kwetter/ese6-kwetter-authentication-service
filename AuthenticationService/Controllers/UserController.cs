using System.Threading.Tasks;
using AuthenticationService.Services;
using AuthenticationService.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("register/password")]
        public async Task<IActionResult> RegisterPassword([FromBody] RegisterModel model)
        {
            var user = await _service.RegisterPassword(model.Email, model.Username, model.Password);

            if (user == null)
                return BadRequest();

            return Ok(user);
        }

        [HttpPost("register/google")]
        public async Task<IActionResult> RegisterGoogle([FromBody] RegisterModel model)
        {
            var user = await _service.RegisterGoogle();

            if (user == null)
                return BadRequest();

            return Ok(user);
        }

        [HttpPost("register/apple")]
        public async Task<IActionResult> RegisterApple([FromBody] RegisterModel model)
        {
            var user = await _service.RegisterApple();

            if (user == null)
                return BadRequest();

            return Ok(user);
        }
    }
}
