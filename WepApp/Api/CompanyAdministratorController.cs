using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateProfile(Company model)
        {
            try
            {
                User user = await Request.GetUser();
                model.UserId = user.Id;
                var profile = await administrator.GetProfileByUserId(user.Id);
                if (profile != null)
                    throw new SystemException("Profile Is Exists ..!");

                profile = await administrator.CreateProfile(model);
                if (profile == null)
                    return BadRequest(new { message = "Create Company Profile Invalid ..!" });
                return Ok(profile);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                User user = await Request.GetUser();
                var profile = await administrator.GetProfileByUserId(user.Id);
                if (profile == null)
                    return BadRequest(new { message = "Company Profile Not Created ..!" });

                return Ok(profile);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        #region trucks
        [HttpPost("trucks")]
        public async Task<IActionResult> AddTruck(Truck truck)
        {
            try
            {
                var adminUser = await Request.GetUser();
                var company = await adminUser.GetCompany();
                truck.CompanyId = company.Id;
                ITruck result = await administrator.AddNewTruck(truck);
                return Ok(result);
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
                return Ok(result.ToList<ITruck>());
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPut("trucks")]
        public async Task<IActionResult> PutTrucks(Truck truck)
        {
            try
            {
                var adminUser = await Request.GetUser();
                var company = await adminUser.GetCompany();
                var result = await administrator.UpdateTrucks(truck);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }





        #endregion


        #region submission


        [HttpPost("createsubmission")]
        public async Task<IActionResult> createsubmission(Pengajuan pengajuan)

        {
            try
            {
                var result = await administrator.AddNewPengajuanTruck(pengajuan);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet("submission")]
        public async Task<IActionResult> GetSubmission()
        {

            try
            {
                var adminUser = await Request.GetUser();
                var company = await adminUser.GetCompany();
                var result = await administrator.GetSubmissionByCompanyId(company.Id);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("submission/{id}")]
        public async Task<IActionResult> GetSubmission(int id)
        {

            try
            {
                var result = await administrator.GetSubmissionByPengajuanId(id);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}