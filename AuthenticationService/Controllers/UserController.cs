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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = await _service.Register(model.Username, model.Password, model.Email);

            if (user == null)
                return BadRequest();

            return Ok(user);
        }
    }
}
