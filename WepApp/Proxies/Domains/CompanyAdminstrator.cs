using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Proxy.Domains
{
    public interface ICompanyAdministrator
    {
        Task<Company> CreateProfile(Company model);
        Task<Company> UpdateProfile(Company model);
        Task<Truck> AddNewTruck(Truck truck);
        Task<Pengajuan> AddNewPengajuanTruck(Pengajuan pengajuan);
        Task<Pengajuan> UpdatePengajuanTruck(int id, Pengajuan pengajuan);
        Task<IEnumerable<KIM>> GetAllKim(int companyId);
        Task<bool> ChangeQRPPejabat(int companyId);
        Task<List<Truck>> GetTrucks(int id);
        Task<Company> GetProfileByUserId(int id);
        Task<Pengajuan> GetSubmissionByPengajuanId(int id);
        Task<List<Pengajuan>> GetSubmissionByCompanyId(int id);
        Task<bool> UpdateTrucks(Truck truck);
        Task<bool> CancelPengajuan(int id);
        Task<object> GetDashboard(int companyId);
    }

    public class CompanyAdministrator : ICompanyAdministrator
    {
        private DataContext context = GetServiceProvider.Instance.GetRequiredService<DataContext>();
       
        public async Task<Pengajuan> AddNewPengajuanTruck(Pengajuan pengajuan)
        {
            try
            {
                context.ChangeTracker.Clear();
                if(pengajuan.Items==null || pengajuan.Items.Count<=0)
                    throw new SystemException("Data Kendaraan Tidak Boleh Kosong !");

                pengajuan.Created = DateTime.Now;
                foreach (var item in pengajuan.Items)
                {
                    var pItem = context.PengajuanItems
                    .Include(x=>x.Truck)
                    .Include(x=>x.Persetujuans)
                    .Where(x=>x.Truck.Id==item.Truck.Id)
                    .AsNoTracking()
                    .ToList();
                    
                    var pengajuanItem= pItem.Where(x=>x.Status == Helpers.StatusPersetujuan.Proccess || x.Status== Helpers.StatusPersetujuan.Approved || x.Status== Helpers.StatusPersetujuan.Reject).FirstOrDefault();
                     if(pengajuanItem!=null){
                         throw new SystemException($"Mobil : {item.Truck.PlateNumber} Sudah Diajukan  !");   
                     }                   


                    context.Entry(item.Truck).State = EntityState.Unchanged;
                }

                context.Entry(pengajuan.Company).State = EntityState.Unchanged;
                context.Attach(pengajuan);
                context.Pengajuans.Add(pengajuan);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                    return pengajuan;
                throw new SystemException("Pengajuan Not Saved ...!");
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }


        public async Task<Pengajuan> UpdatePengajuanTruck(int id, Pengajuan pengajuan)
        {
            try
            {
                var oldPengajuan = context.Pengajuans.Include(x=>x.Items).Where(x => x.Id == id).FirstOrDefault();
                if (oldPengajuan == null)
                    throw new SystemException("Data Tidak Ditemukan !");

                oldPengajuan.LetterNumber = pengajuan.LetterNumber;
                oldPengajuan.StatusPengajuan = pengajuan.StatusPengajuan;

                foreach (var item in pengajuan.Items.ToList())
                {
                    if(item.Id == 0)
                    {
                        context.Entry(item.Truck).State = EntityState.Unchanged;
                        oldPengajuan.Items.Add(item);
                    }
                    else if(oldPengajuan.Items!=null)
                    {
                        var oldItem = oldPengajuan.Items.Where(x => x.Id == item.Id).FirstOrDefault();
                        if (oldItem != null && oldItem.AttackStatus != item.AttackStatus)
                        {
                            oldItem.AttackStatus = item.AttackStatus;
                        }
                    }
                }


                foreach (var item in oldPengajuan.Items.ToList())
                {
                    var removeItem = pengajuan.Items.Where(x => x.Id == item.Id).FirstOrDefault();
                    if (removeItem == null)
                    {
                        context.PengajuanItems.Remove(item);
                    }
                }


                var result = await context.SaveChangesAsync();
                if (result > 0)
                    return pengajuan;
                throw new SystemException("Pengajuan Not Saved ...!");
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<Truck> AddNewTruck(Truck truck)
        {
            try
            {
                var dataTruck = await SaveFile(truck);
                context.Trucks.Add(dataTruck);
                var result = await context.SaveChangesAsync();
                if (result > 0)
                    return truck;
                throw new SystemException("Truck Not Saved ...!");
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<bool> ChangeQRPPejabat(int companyId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Company> CreateProfile(Company profile)
        {
            try
            {

                var tracker = context.ChangeTracker.Entries();

                if (profile.LogoData != null)
                {
                    var logoFile = await Helpers.FileHelper.SaveTruckFile(profile.LogoData, Helpers.PathType.Photo, profile.Logo);
                    profile.Logo = logoFile;
                }
                if(profile.User!=null){
                    context.Entry<User>(profile.User).State= EntityState.Unchanged;
                }

                context.Companies.Add(profile);
                await context.SaveChangesAsync();

                return profile;

            }
            catch (System.Exception ex)
            {
                throw new System.SystemException(ex.Message);
            }

        }

        public async Task<Company> UpdateProfile(Company profile)
        {
            try
            {
                var data = context.Companies.Where(x => x.Id == profile.Id).FirstOrDefault();
                if (data == null)
                    throw new SystemException("Profile Not Found ...!");

                if (profile.LogoData != null)
                {
                    var logoFile = await Helpers.FileHelper.SaveTruckFile(profile.LogoData, Helpers.PathType.Photo, profile.Logo);
                    profile.Logo = logoFile;
                }
                context.Entry(data).CurrentValues.SetValues(profile);
                await context.SaveChangesAsync();
                return profile;
            }
            catch (System.Exception ex)
            {
                throw new System.SystemException(ex.Message);
            }
        }

        public Task<IEnumerable<KIM>> GetAllKim(int companyId)
        {
            var results = context.Kims.Include(x => x.Truck).ThenInclude(x => x.Company).Where(x => x.Truck.Company.Id == companyId);
            return Task.FromResult(results.AsNoTracking().AsEnumerable());
        }

        public Task<Company> GetProfileByUserId(int id)
        {
            Company profile = context.Companies.Include(x=>x.User).Where(x => x.User.Id == id).AsNoTracking().FirstOrDefault();
            return Task.FromResult(profile);
        }

        public Task<Pengajuan> GetSubmissionByPengajuanId(int id)
        {

            var result = context.Pengajuans.Include(x=>x.Company)
            .Include(x => x.Items).ThenInclude(x=>x.HasilPemeriksaan)
            .ThenInclude(x=>x.ItemPemeriksaan).ThenInclude(x=>x.Pemeriksaan)
            .Include(x => x.Items).ThenInclude(x=>x.Persetujuans)
            .Include(x => x.Items).ThenInclude(x=>x.Truck).Where(x => x.Id == id);
            return Task.FromResult(result.AsNoTracking().FirstOrDefault());
        }


        public Task<List<Pengajuan>> GetSubmissionByCompanyId(int id)
        {
            var result = context.Pengajuans.Include(x=>x.Company)
            .Where(x => x.Company.Id == id)
            .Include(x => x.Items).ThenInclude(x=>x.HasilPemeriksaan)
            .ThenInclude(x=>x.ItemPemeriksaan)
            .ThenInclude(x=>x.Pemeriksaan)
            .Include(x => x.Items).ThenInclude(x=>x.Persetujuans).ThenInclude(x=>x.User)
            .Include(x => x.Items).ThenInclude(x=>x.Truck);
            return Task.FromResult(result.AsNoTracking().ToList());
        }

        public Task<List<Truck>> GetTrucks(int companyId)
        {
            var result = context.Trucks
            .Include(x=>x.Company)
            .Include(x=>x.Kims)
            .Where(x => x.CompanyId == companyId);
            return Task.FromResult(result.AsNoTracking().ToList());
        }

        public async Task<bool> UpdateTrucks(Truck data)
        {
            try
            {
                var truck = await SaveFile(data);

                var truckExists = context.Trucks.Where(x => x.Id == truck.Id).FirstOrDefault();
                if (truckExists == null)
                    throw new System.SystemException("Truck Not Found !");

                truckExists.AssdriverIDCard = truck.AssdriverIDCard;
                truckExists.AssdriverLicense = truck.AssdriverLicense;
                truckExists.AssdriverName = truck.AssdriverName;
                truckExists.AssdriverPhoto = truck.AssdriverPhoto;
                truckExists.DriverIDCard = truck.DriverIDCard;
                truckExists.DriverLicense = truck.DriverLicense;
                truckExists.DriverName = truck.DriverName;
                truckExists.DriverPhoto = truck.DriverPhoto;
                truckExists.TruckPhoto = truck.TruckPhoto;
                truckExists.KeurDLLAJR = truck.KeurDLLAJR;
                truckExists.VehicleRegistration = truck.VehicleRegistration;
                truckExists.Merk = truck.Merk;
                truckExists.CarCreated = truck.CarCreated;
                truckExists.PlateNumber = truck.PlateNumber;
                truckExists.Merk = truck.Merk;
                truckExists.TruckType = truck.TruckType;

                var saved = await context.SaveChangesAsync();
                if (saved > 0)
                    return true;
                return false;


            }
            catch (System.Exception ex)
            {

                throw new System.SystemException(ex.Message);
            }


        }


        private async Task<Truck> SaveFile(Truck truck)
        {
            List<Tuple<int, Task<string>>> tasks = new List<Tuple<int, Task<string>>>();
            var taskDriverId = Helpers.FileHelper.SaveTruckFile(truck.FileDriverId, Helpers.PathType.Document, truck.DriverIDCard==null?string.Empty: truck.DriverIDCard.Document);
            tasks.Add(Tuple.Create(1, taskDriverId));

            var taskAssDriverId = Helpers.FileHelper.SaveTruckFile(truck.FileAssDriverId, Helpers.PathType.Document, truck.AssdriverIDCard==null?string.Empty: truck.AssdriverIDCard.Document);
            tasks.Add(Tuple.Create(2, taskAssDriverId));

            var taskDriverPhoto = Helpers.FileHelper.SaveTruckFile(truck.FileDriverPhoto, Helpers.PathType.Photo, truck.DriverPhoto);
            tasks.Add(Tuple.Create(3, taskDriverPhoto));

            var taskassDriverPhoto = Helpers.FileHelper.SaveTruckFile(truck.FileAssDriverPhoto, Helpers.PathType.Photo, truck.AssdriverPhoto);
            tasks.Add(Tuple.Create(4, taskassDriverPhoto));

            var taskDriverLicense = Helpers.FileHelper.SaveTruckFile(truck.FileDriverLicense, Helpers.PathType.Document, truck.DriverLicense==null?string.Empty: truck.DriverLicense.Document);
            tasks.Add(Tuple.Create(5, taskDriverLicense));

            var taskAssDriverLicense = Helpers.FileHelper.SaveTruckFile(truck.FileAssDriverLicense, Helpers.PathType.Document, truck.AssdriverLicense==null?string.Empty: truck.AssdriverLicense.Document);
            tasks.Add(Tuple.Create(6, taskAssDriverLicense));

            var taskVichel = Helpers.FileHelper.SaveTruckFile(truck.FileVehicleRegistration, Helpers.PathType.Document, truck.VehicleRegistration==null?string.Empty: truck.VehicleRegistration.Document);
            tasks.Add(Tuple.Create(7, taskVichel));

            var taskKeur = Helpers.FileHelper.SaveTruckFile(truck.FileKeurDLLAJR, Helpers.PathType.Document, truck.KeurDLLAJR==null?string.Empty: truck.KeurDLLAJR.Document);
            tasks.Add(Tuple.Create(8, taskKeur));

            var tasktruckPhoto = Helpers.FileHelper.SaveTruckFile(truck.FileKeurDLLAJR, Helpers.PathType.Photo, truck.TruckPhoto);
            tasks.Add(Tuple.Create(9, tasktruckPhoto));

            var resultTask = await Task.WhenAll(tasks.Select(x => x.Item2).ToArray());
            foreach (var item in tasks)
            {
                var filename = await item.Item2;
                    if (!string.IsNullOrEmpty(filename))
                    {
                        switch (item.Item1)
                        {
                            case 1:
                            truck.DriverIDCard.Document = filename;
                            break;
                        case 2:
                            truck.AssdriverIDCard.Document = filename;
                            break;
                        case 3:
                            truck.DriverPhoto = filename;
                            break;
                        case 4:
                            truck.AssdriverPhoto = filename;
                            break;
                        case 5:
                            truck.DriverLicense.Document = filename;
                            break;
                        case 6:
                            truck.AssdriverLicense.Document = filename;
                            break;
                        case 7:
                            truck.VehicleRegistration.Document = filename;
                            break;
                        case 8:
                            truck.KeurDLLAJR.Document = filename;
                            break;
                        case 9:
                            truck.TruckPhoto = filename;
                            break;

                    }
                }
            }

            return truck;
        }

        public async Task<bool> CancelPengajuan(int id)
        {
            try
            {
                var data = context.Pengajuans.Where(x => x.Id == id).FirstOrDefault();
                context.Attach(data).State = EntityState.Modified;
                data.StatusPengajuan = Helpers.StatusPengajuan.Cancel;
                await context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<object> GetDashboard(int companyId)
        {
           var trucks = context.Trucks.Where(x=>x.CompanyId==companyId);
           var kims = trucks.Include(x=>x.Kims).ToList().Where(x=>x.KIM!=null).Count();
           var truck = trucks.Count();
           var pengajuans = context.PengajuanItems
           .Include(x=>x.Truck).ThenInclude(x=>x.Company)
           .Include(x=>x.Persetujuans).ToList().Where(x=>x.Truck.Company.Id==companyId);
           var terima = pengajuans.Where(x=>x.Status== Helpers.StatusPersetujuan.Complete).Count();
           var proses = pengajuans.Where(x=>x.Status== Helpers.StatusPersetujuan.Proccess).Count();
           var tolak = pengajuans.Where(x=>x.Status== Helpers.StatusPersetujuan.Reject).Count();
           object data = new {Truck=truck, Kim=kims, Pengajuan=pengajuans.Count(), Terima=terima, Tolak=tolak, Proses=proses};
           return Task.FromResult(data);
        }


        

    }
}