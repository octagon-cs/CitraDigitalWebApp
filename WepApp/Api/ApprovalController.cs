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
    [Authorize(Roles = "Administrator")]
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
              var pengajuans= await approval.GetPengajuanNotApprove();

              return Ok(pengajuans);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}