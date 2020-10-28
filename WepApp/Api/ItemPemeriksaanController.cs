using System;
using System.Linq;
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

    public class ItemPemeriksaanController : ControllerBase
    {
        private IUserService _userService;
        private DataContext _context;

        public ItemPemeriksaanController(IUserService userService, DataContext context)
        {
            _userService = userService;
            _context = context;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var items = _context.ItemPemeriksaans.ToList();
                return Ok(items);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Administrator")]

        [HttpPost]
        public async Task<IActionResult> Post(ItemPemeriksaan model)
        {
            try
            {
                var admin = UserProxy.GetAdministratorProxy(_userService);
                var result = await admin.AddNewItemPemeriksaaan(model);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "Administrator")]

        [HttpPut]
        public async Task<IActionResult> Put(int id, ItemPemeriksaan model)
        {
            try
            {
                var admin = UserProxy.GetAdministratorProxy(_userService);
                var result = await admin.UpdateItemPemeriksaan(id, model);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}