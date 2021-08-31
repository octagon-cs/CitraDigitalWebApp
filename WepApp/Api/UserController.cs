using System.Collections.Generic;
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

                if(!response.Status){
                    return Unauthorized(new { message = "You are account not active, Contact Administrator" });
                }

                return Ok(response);
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _userService.GetAll();
                var listUsers = new List<object>();
                
                foreach (var item in response)
                {
                    listUsers.Add(new {FirstName=item.FirstName, Username=item.UserName, Email=item.Email,item.LastName, item.Status, item.UserRoles });
                }

                return Ok(listUsers);
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put(int id, User user)
        {
            try
            {
                var response = await _userService.UpdateUser(id, user);
                return Ok(response);
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}