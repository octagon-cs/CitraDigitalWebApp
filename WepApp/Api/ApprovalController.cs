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
    [Authorize(Roles = "Approval1, Manager, HSE")]
    public class ApprovalController : ControllerBase
    {
        private IApproval approval;

        [HttpGet("GetPersetujuan")]
        public async Task<IActionResult> GetPersetujuan()
        {
            try
            {
                var user = await Request.GetUser();
                approval = UserProxy.GetApprovalProxy(user);
                var pengajuans = await approval.GetPengajuanNotApprove();
                return Ok(pengajuans);
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
                var user = await Request.GetUser();
                approval = UserProxy.GetApprovalProxy(user);
                var pengajuan = await approval.Approve(id, model);
                return Ok(pengajuan);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetPenilaian/{id}")]
        public async Task<IActionResult> GetPenilaian(int id)
        {
            try
            {
                var user = await Request.GetUser();
                approval = UserProxy.GetApprovalProxy(user);
                var penilaian = await approval.GetPenilaian(id);
                return Ok(penilaian);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}