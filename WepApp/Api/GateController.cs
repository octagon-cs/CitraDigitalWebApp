using System;
using System.Collections.Generic;
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
    [Authorize(Roles = "Gate")]
    public class GateController : ControllerBase
    {
        private IGateAdministrator administrator;

        public GateController()
        {
            administrator = UserProxy.GetGateProxy();
        }


        [HttpGet("trucks")]
        public async Task<IActionResult> GetTrucks()
        {
            try
            {
                var result = await administrator.GetTrucks();
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("trucklastcheckup/{id}")]
        public async Task<IActionResult> GetLastCheckUp(int id)
        {
            try
            {
                var result = await administrator.GetLastPengajuan(id);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("approve/{id}")]
        public async Task<IActionResult> Approve(int id, List<HasilPemeriksaan> model)
        {
            try
            {
                var result = await administrator.Approve(id, model, true);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> Reject(int id, List<HasilPemeriksaan> model)
        {
            try
            {
                var result = await administrator.Approve(id, model, false);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("TruckHistories/{id}")]
        public async Task<IActionResult> TruckHistories(int id)
        {
            try
            {
                ITruck result = (ITruck)await administrator.TruckHistrories(id);
                return Ok(result as ITruck);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}