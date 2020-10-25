using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
          try
          {
                var response = await _userService.Authenticate(model);
            if (response == null)
            {
                return Unauthorized(new { message = "Username or password is incorrect" });
            }

            return Ok(response);
          }
          catch (System.Exception ex)
          {
              
              return BadRequest(ex.Message);
          }
        }

    }
}