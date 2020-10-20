using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataStores;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.Proxy;
using WebApp.Proxy.Domains;
using WebApp.Services;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Company")]
    public class CompanyAdministratorController : ControllerBase
    {
        private ICompanyAdministrator administrator;

        public CompanyAdministratorController(IUserService userService)
        {
              administrator = UserProxy.GetCompanyAdministratorProxy();
        }

        [HttpPost("CreateProfile")]
        public async Task<IActionResult> CreateProfile(CompanyProfile model)
        {
            try
            {
                var user = await administrator.CreateProfile(model);
                if (user == null)
                    return BadRequest(new { message = "Username or password is incorrect" });
                return Ok(user);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("addtruck")]
        public async Task<IActionResult> AddTruck(Truck truck)
        {
            try
            {
                await administrator.AddNewTruck(truck);
                return Ok(true);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}