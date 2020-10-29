using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Proxy.Domains
{
    public interface IAdministrator
    {
        Task<User> CreateUser(User user);
        Task AddUserRole(int userId, string roleName);
        Task<ItemPemeriksaan> AddNewItemPemeriksaaan(ItemPemeriksaan item);
        Task<List<KIM>> GetAllKIMNotYetApproved();

        Task<KIM> CreateNewKIM(int itempengajuanId, KIM kim);

        Task<KIM> GetKIMById(int kimId);

        Task PrintKIM(KIM kim);

        Task<List<KIM>> GetAllKIM();
        Task<bool> UpdateItemPemeriksaan(int id, ItemPemeriksaan model);
        Task<List<PengajuanItem>> GetPersetujuan();
    }


    public class Administrator : IAdministrator
    {
        private IUserService _userService;
        private DataContext _context;

        public Administrator(IUserService userService, DataContext context)
        {
            _userService = userService;
            _context = context;

        }
        public async Task<ItemPemeriksaan> AddNewItemPemeriksaaan(ItemPemeriksaan item)
        {
            try
            {
                _context.ItemPemeriksaans.Add(item);
                await _context.SaveChangesAsync();
                return item;

            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task AddUserRole(int userId, string roleName)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                if (user != null)
                {
                    var role = _context.Roles.Where(x => x.Name == roleName).FirstOrDefault();
                    user.UserRoles.Add(new UserRole { UserId = userId, RoleId = role.Id });
                }
                await _context.SaveChangesAsync();

            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }

        }

        public async Task<KIM> CreateNewKIM(int itempengajuanid, KIM kim)
        {
            var pengajuanItem = _context.PengajuanItems.Where(xx => xx.Id == itempengajuanid)
              .Include(x => x.Truck)
            .AsNoTracking().FirstOrDefault();

            var truck = _context.Trucks.Where(xx => xx.Id == pengajuanItem.TruckId).Include(xx => xx.Kims).FirstOrDefault();
            KIM tempKim = truck.KIM;
            if (tempKim == null)
            {
                tempKim = kim;
                tempKim.TruckId = pengajuanItem.TruckId;
                _context.Kims.Add(tempKim);
            }
            else
            {
                var dataKim = _context.Kims.Where(x => x.Id == tempKim.Id).FirstOrDefault();
                dataKim.BeginDate = kim.BeginDate;
                dataKim.EndDate = kim.EndDate;
            }
            var result = await _context.SaveChangesAsync();

            return tempKim;
        }

        public Task<User> CreateUser(User user)
        {
            try
            {
                var result = _userService.Register(user);
                if (result != null)
                    return result;
                return null;

            }
            catch (System.Exception ex)
            {

                throw new System.SystemException(ex.Message);
            }
        }

        public Task<List<KIM>> GetAllKIM()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<KIM>> GetAllKIMNotYetApproved()
        {
            throw new System.NotImplementedException();
        }

        public Task<KIM> GetKIMById(int kimId)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<PengajuanItem>> GetPersetujuan()
        {
            var pengajuans = _context.Pengajuans.Where(x => x.StatusPengajuan != Helpers.StatusPengajuan.Complete)
            .Include(x => x.Items).ThenInclude(x => x.Truck)
            .Include(x => x.Items).ThenInclude(x => x.Persetujuans)
            .Include(x => x.Items).ThenInclude(x => x.HasilPemeriksaan)
            .AsNoTracking();
            var resuls = pengajuans.SelectMany(x => x.Items).ToList().Where(x => x.Status == Helpers.StatusPersetujuan.Approved);


            List<PengajuanItem> list = new List<PengajuanItem>();
            foreach (var item in resuls)
            {
                var truck = _context.Trucks.Where(x => x.Id == item.TruckId).Include(x => x.Kims).FirstOrDefault();
                if (truck.KIM == null || (truck.KIM != null && truck.KIM.Expired != Helpers.ExpireStatus.None))
                {
                    list.Add(item);
                }
            }

            return Task.FromResult(list.ToList());
        }

        public Task PrintKIM(KIM kim)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateItemPemeriksaan(int id, ItemPemeriksaan model)
        {
            try
            {
                var item = _context.ItemPemeriksaans.Where(x => x.Id == id).FirstOrDefault();
                if (item == null)
                    throw new SystemException("Item Pemeriksaan Not Found !");
                item.Kelengkapan = model.Kelengkapan;
                item.Penjelasan = model.Penjelasan;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return true;
                return false;
            }
            catch (System.Exception ex)
            {

                throw new SystemException(ex.Message);
            }
        }
    }
}