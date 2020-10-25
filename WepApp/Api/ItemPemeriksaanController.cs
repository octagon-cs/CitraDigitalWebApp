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
    public class ItemPemeriksaanController : ControllerBase
    {
        private IUserService _userService;

        public ItemPemeriksaanController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("AddItemPemeriksaan")]
        public async Task<IActionResult> AddItemPemeriksaan(ItemPemeriksaan model)
        {
            try
            {
              var user = await Request.GetUser();
              var admin = UserProxy.GetAdministratorProxy(_userService);
             var result = await admin.AddNewItemPemeriksaaan(model);
              return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}