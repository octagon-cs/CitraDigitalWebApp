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
                User user = await Request.GetUser();
                model.UserId = user.Id;
                var profile = await administrator.CreateProfile(model);
                if (profile == null)
                    return BadRequest(new { message = "Create Company Profile Invalid ..!" });
                return Ok(profile);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addtruck")]
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

        [HttpGet("trucks")]
        public async Task<IActionResult> GetTrucks()
        {
            try
            {
                var adminUser = await Request.GetUser();    
                var company = await adminUser.GetCompany();
                var result = await administrator.GetTrucks(company.Id);
                return Ok(true);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("createsubmission")]
        public async Task<IActionResult> createsubmission(Pengajuan pengajuan)
        {
            try
            {
                await administrator.AddNewPengajuanTruck(pengajuan);
                return Ok(true);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}