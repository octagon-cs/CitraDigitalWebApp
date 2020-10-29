using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.Proxy;
using WebApp.Proxy.Domains;
using WebApp.Services;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : ControllerBase
    {
        private IAdministrator administrator;

        public AdministratorController(IUserService userService)
        {
            administrator = UserProxy.GetAdministratorProxy(userService);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(User model)
        {
            try
            {
                var user = await administrator.CreateUser(model);
                if (user == null)
                    return BadRequest(new { message = "Username or password is incorrect" });
                return Ok(user);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("adduserrole")]
        public async Task<IActionResult> AddUserRole(int userId, string roleName)
        {
            try
            {
                await administrator.AddUserRole(userId, roleName);
                return Ok(true);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet("GetPersetujuan")]
        public async Task<IActionResult> GetPersetujuan()
        {
            try
            {
                var results = await administrator.GetPersetujuan();
                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("CreateKim/{id}")]
        public async Task<IActionResult> CreateKim(int id, KIM kim)
        {
            try
            {
                var user = await administrator.CreateNewKIM(id, kim);
                if (user == null)
                    return BadRequest(new { message = "Username or password is incorrect" });
                return Ok(user);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}