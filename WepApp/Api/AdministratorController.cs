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
        private IUserService _userService;
        private IAdministrator administrator;

        public AdministratorController(IUserService userService)
        {
            _userService = userService;
            administrator = UserProxy.GetAdministratorProxy(null, userService);
        }

        [HttpPost("CreateUser/{rolename}")]
        public async Task<IActionResult> CreateUser(string roleName, User model)
        {
            try
            {

                var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

                var user = await administrator.CreateUser(baseUrl, roleName, model);
                if (user == null)
                    return BadRequest(new { message = "Gagal Tambah User !" });
                return Ok(user);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, User model)
        {
            try
            {
                var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                var user = await administrator.UpdateUser(baseUrl, id, model);
                if (user == null)
                    return BadRequest(new { message = "Not Saved ...!" });
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
                var user = await Request.GetUser();
                administrator = UserProxy.GetAdministratorProxy(user, _userService);
                var model = await administrator.CreateNewKIM(id, kim);
                if (model == null)
                    return BadRequest(new { message = "Gagal Buat KIM" });
                return Ok(model);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetKims")]
        public async Task<IActionResult> GetKims()
        {
            try
            {
                var kims = await administrator.GetAllKIM();
                if (kims == null)
                    return NotFound(new { message = "Data Kim Tidak Ditemukan !" });
                return Ok(kims);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetKims/{id}")]
        public async Task<IActionResult> GetKims(int id)
        {
            try
            {
                var kim = await administrator.GetKIMById(id);
                if (kim == null)
                    return BadRequest(new { message = "KIM Not Found ...!" });
                return Ok(kim);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetManager")]
        public async Task<IActionResult> GetManagerName(int id)
        {
            try
            {
                string nama = await administrator.GetManagerName(id);
                if (string.IsNullOrEmpty(nama))
                    return BadRequest(new { message = "Manager aktif belum ada !" });
                return Ok(nama);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

         [HttpGet("GetDashboard")]
        public async Task<IActionResult> GetDasboard(int id)
        {
            try
            {
                object data = await administrator.GetDashboard();
                return Ok(data);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //  [HttpGet("submission")]
        // public async Task<IActionResult> GetSubmision()
        // {
        //     try
        //     {
        //         object data = await administrator.GetPengajuan();
        //         return Ok(data);
        //     }
        //     catch (System.Exception ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        // }
        //  [HttpGet("submission/{id}")]
        // public async Task<IActionResult> GetSubmisionById(int id)
        // {
        //     try
        //     {
        //         object data = await administrator.GetPengajuanById(id);
        //         return Ok(data);
        //     }
        //     catch (System.Exception ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        // }
    }
}