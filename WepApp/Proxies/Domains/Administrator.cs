using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Services;
using WebApp.Helpers;

namespace WebApp.Proxy.Domains
{
    public interface IAdministrator
    {
        Task<User> CreateUser(string roleName, User user);
        Task<User> UpdateUser(int id, User user);
        Task AddUserRole(int userId, string roleName);
        Task<Pemeriksaan> AddNewItemPemeriksaaan(Pemeriksaan item);
        Task<List<KIM>> GetAllKIMNotYetApproved();
        Task<KIM> CreateNewKIM(int itempengajuanId, KIM kim);
        Task<KIM> GetKIMById(int kimId);
        Task PrintKIM(KIM kim);
        Task<List<KIM>> GetAllKIM();
        Task<bool> UpdateItemPemeriksaan(int id, Pemeriksaan model);
        Task<List<PengajuanItem>> GetPersetujuan();
        // Task<List<Pengajuan>> GetPengajuan();
        // Task<Pengajuan> GetPengajuanById(int id);
        Task<string> GetManagerName(int id);
        Task<object> GetDashboard();
    }


    public class Administrator : IAdministrator
    {
        private User _userLogin;
        private IUserService _userService;
        private DataContext _context;
        public Administrator(IUserService userService, DataContext context)
        {
            _userService = userService;
            _context = context;

        }
        public Administrator(User userLogin, IUserService userService, DataContext context)
        {
            _userLogin = userLogin;
            _userService = userService;
            _context = context;

        }

        public async Task<User> UpdateUser(int id, User user)
        {
            try
            {
                var data = _context.Users.Where(x => x.Id == user.Id).FirstOrDefault();
                _context.Attach(data).State = EntityState.Modified;
                data.FirstName = user.FirstName;
                data.LastName = user.LastName;
                data.Email = user.Email;
                data.Status = user.Status;
                var result = await _context.SaveChangesAsync();
                if (result <= 0)
                    throw new SystemException("Not Saved ...!");
                return user;

            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<Pemeriksaan> AddNewItemPemeriksaaan(Pemeriksaan item)
        {
            try
            {
                _context.Pemeriksaans.Add(item);
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
                    user.UserRoles.Add(new UserRole { Role = role, User = user });
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
            try
            {
                var pengajuanItem = _context.PengajuanItems.Where(xx => xx.Id == itempengajuanid)
                  .Include(x => x.Truck).FirstOrDefault();
                var tracker = _context.ChangeTracker.Entries();
                var truck = _context.Trucks.Where(xx => xx.Id == pengajuanItem.Truck.Id).Include(xx => xx.Kims).FirstOrDefault();
                _context.ChangeTracker.Clear();
                _context.Attach(truck);
                truck.Kims.Add(kim);
                var persetujuan = new Persetujuan { User = _userLogin, StatusPersetujuan = Helpers.StatusPersetujuan.Complete, ApprovedBy = Helpers.UserType.Administrator, ApprovedDate = DateTime.Now };
                _context.Entry(persetujuan.User).State = EntityState.Unchanged;
                _context.Attach(pengajuanItem);
                pengajuanItem.Persetujuans.Add(persetujuan);
                var result = await _context.SaveChangesAsync();
                return kim;
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<User> CreateUser(string roleName, User user)
        {
            try
            {
                var result = await _userService.Register(roleName, user);
                if (result != null)
                    return result;
                return null;

            }
            catch (System.Exception ex)
            {

                throw new System.SystemException(ex.Message);
            }
        }

        public async Task<List<KIM>> GetAllKIM()
        {
            var kims = await _context.Kims
            .Include(x => x.Truck).ThenInclude(x => x.Company)
            .AsNoTracking().ToListAsync();
            return kims.ToList();
        }

        public Task<List<KIM>> GetAllKIMNotYetApproved()
        {
            throw new System.NotImplementedException();
        }

        public Task<KIM> GetKIMById(int kimId)
        {
            var kims = _context.Kims.Where(x => x.Id == kimId).FirstOrDefault();
            return Task.FromResult(kims);
        }

        public Task<List<PengajuanItem>> GetPersetujuan()
        {
            IEnumerable<PengajuanItem> pengajuanStrore = _context.PengajuanItems
          .Include(x => x.Persetujuans)
          .Include(x => x.Truck).ThenInclude(x => x.Kims)
          .Include(x => x.HasilPemeriksaan)
          .Include(x => x.Pengajuan).ThenInclude(x => x.Company).AsNoTracking().ToList();
            var results = pengajuanStrore.Where(x => x.NextApprove == UserType.Administrator).ToList();
            return Task.FromResult(results);
        }

        public Task PrintKIM(KIM kim)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateItemPemeriksaan(int id, Pemeriksaan model)
        {
            try
            {
                _context.ChangeTracker.Clear();
                var item = _context.Pemeriksaans.Include(x => x.Items).FirstOrDefault(x => x.Id == id);
                if (item == null)
                    throw new SystemException("Item Pemeriksaan Not Found !");

                item.Name = model.Name;
                foreach (var data in model.Items)
                {
                    if (data.Id == 0)
                        item.Items.Add(data);
                    else
                    {
                        var existingChild = item.Items
                                               .Where(c => c.Id == data.Id)
                                                   .SingleOrDefault();

                        if (existingChild != null)
                            _context.Entry(existingChild).CurrentValues.SetValues(data);
                        else
                        {
                            item.Items.Add(data);
                        }
                    }
                }

                foreach (var data in item.Items.ToList())
                {
                    var existingChild = model.Items
                                               .SingleOrDefault(c => c.Id == data.Id);
                    if (existingChild == null)
                    {
                        item.Items.Remove(data);
                    }
                }

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

        public Task<string> GetManagerName(int id)
        {
            try
            {
                var manager = _context.Users.Where(x => x.UserType == Helpers.UserType.Manager && x.Status).AsNoTracking().FirstOrDefault();
                if (manager == null)
                    return null;
                return Task.FromResult(manager.FullName);
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<object> GetDashboard()
        {
            var trucks = _context.Trucks.Count();
            var pengajuans = _context.PengajuanItems.Include(x => x.Persetujuans).ToList();
            var terima = pengajuans.Where(x => x.Status == Helpers.StatusPersetujuan.Complete).Count();
            var proses = pengajuans.Where(x => x.Status == Helpers.StatusPersetujuan.Proccess).Count();
            var tolak = pengajuans.Where(x => x.Status == Helpers.StatusPersetujuan.Reject).Count();
            var kims = _context.Trucks.Include(x => x.Kims).ToList().Where(x => x.KIM != null).Count();
            object data = new { Truck = trucks, Kim = kims, Pengajuan = pengajuans.Count, Terima = terima, Tolak = tolak, Proses = proses };
            return Task.FromResult(data);
        }

        //   public Task<List<Pengajuan>> GetPengajuan(){
        //     var result = _context.Pengajuans.Include(x=>x.Company)
        //     .Include(x => x.Items).ThenInclude(x=>x.HasilPemeriksaan)
        //     .ThenInclude(x=>x.ItemPemeriksaan).ThenInclude(x=>x.Pemeriksaan)
        //     .Include(x => x.Items).ThenInclude(x=>x.Persetujuans)
        //     .Include(x => x.Items).ThenInclude(x=>x.Truck).AsNoTracking();
        //     return Task.FromResult(result.ToList());
        //   }
        // public Task<Pengajuan> GetPengajuanById(int id){
        //     var result = _context.Pengajuans.Include(x=>x.Company)
        //     .Include(x => x.Items).ThenInclude(x=>x.HasilPemeriksaan)
        //     .ThenInclude(x=>x.ItemPemeriksaan).ThenInclude(x=>x.Pemeriksaan)
        //     .Include(x => x.Items).ThenInclude(x=>x.Persetujuans)
        //     .Include(x => x.Items).ThenInclude(x=>x.Truck).Where(x => x.Id == id).AsNoTracking();
        //     return Task.FromResult(result.FirstOrDefault());

        // }
    }
}