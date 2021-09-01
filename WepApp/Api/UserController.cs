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
                return Ok(response);
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


        [HttpGet("forgotpassword/{email}")]
        public async Task<IActionResult> ForgotPasword(string email)
        {
            try
            {
               var baseUrl= $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                var response = await _userService.ForgotPassword(baseUrl, email);
                return Ok(response);
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            try
            {
                var user = await Request.GetUser();
                var response = await _userService.ChangePassword(user, model);
                return Ok(response);
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


    }
}