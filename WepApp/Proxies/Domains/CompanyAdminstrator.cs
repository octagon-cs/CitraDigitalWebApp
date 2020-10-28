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
        Task<CompanyProfile> CreateProfile(CompanyProfile model);

        Task<Truck> AddNewTruck(Truck truck);

        Task<Pengajuan> AddNewPengajuanTruck(Pengajuan pengajuan);

        Task<IEnumerable<KIM>> GetAllKim(int companyId);

        Task<bool> ChangeQRPPejabat(int companyId);
        Task<IEnumerable<Truck>> GetTrucks(int id);
        Task<CompanyProfile> GetProfileByUserId(int id);
        Task<Pengajuan> GetSubmissionByPengajuanId(int id);
        Task<List<Pengajuan>> GetSubmissionByCompanyId(int id);
        Task<bool> UpdateTrucks(Truck truck);
    }

    public class CompanyAdministrator : ICompanyAdministrator
    {
        private DataContext context = GetServiceProvider.Instance.GetRequiredService<DataContext>();

        public async Task<Pengajuan> AddNewPengajuanTruck(Pengajuan pengajuan)
        {
            try
            {
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

        public async Task<CompanyProfile> CreateProfile(CompanyProfile profile)
        {
            try
            {
                context.CompanyProfiles.Add(profile);
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
            throw new System.NotImplementedException();
        }

        public Task<CompanyProfile> GetProfileByUserId(int id)
        {
            CompanyProfile profile = context.CompanyProfiles.Where(x => x.UserId == id).FirstOrDefault();
            return Task.FromResult(profile);
        }

        public Task<Pengajuan> GetSubmissionByPengajuanId(int id)
        {

            var result = context.Pengajuans.Where(x => x.Id == id).Include(x => x.Items);
            return Task.FromResult(result.FirstOrDefault());
        }


        public Task<List<Pengajuan>> GetSubmissionByCompanyId(int id)
        {

            var result = context.Pengajuans.Where(x => x.CompanyId == id).Include(x => x.Items);
            return Task.FromResult(result.ToList());
        }

        public Task<IEnumerable<Truck>> GetTrucks(int companyId)
        {
            var result = context.Trucks.Where(x => x.CompanyId == companyId);
            return Task.FromResult(result.AsEnumerable());
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
                truckExists.CarCreated = truck.CarCreated;
                truckExists.DriverIDCard = truck.DriverIDCard;
                truckExists.DriverLicense = truck.DriverLicense;
                truckExists.DriverName = truck.DriverName;
                truckExists.DriverPhoto = truck.DriverPhoto;
                truckExists.KeurDLLAJR = truck.KeurDLLAJR;
                truckExists.Merk = truck.Merk;
                truckExists.PlateNumber = truck.PlateNumber;
                truckExists.Merk = truck.Merk;
                truckExists.TruckType = truck.TruckType;
                truckExists.VehicleRegistration = truck.VehicleRegistration;

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
            var taskDriverId = Helpers.FileHelper.SaveTruckFile(truck.FileDriverId, Helpers.PathType.Document, truck.DriverIDCard);
            tasks.Add(Tuple.Create(1, taskDriverId));

            var taskAssDriverId = Helpers.FileHelper.SaveTruckFile(truck.FileAssDriverId, Helpers.PathType.Document, truck.AssdriverIDCard);
            tasks.Add(Tuple.Create(2, taskAssDriverId));

            var taskDriverPhoto = Helpers.FileHelper.SaveTruckFile(truck.FileDriverPhoto, Helpers.PathType.Photo, truck.DriverPhoto);
            tasks.Add(Tuple.Create(3, taskDriverPhoto));

            var taskassDriverPhoto = Helpers.FileHelper.SaveTruckFile(truck.FileAssDriverPhoto, Helpers.PathType.Photo, truck.AssdriverPhoto);
            tasks.Add(Tuple.Create(4, taskassDriverPhoto));

            var taskDriverLicense = Helpers.FileHelper.SaveTruckFile(truck.FileDriverLicense, Helpers.PathType.Document, truck.DriverLicense);
            tasks.Add(Tuple.Create(5, taskDriverLicense));

            var taskAssDriverLicense = Helpers.FileHelper.SaveTruckFile(truck.FileAssDriverLicense, Helpers.PathType.Document, truck.AssdriverLicense);
            tasks.Add(Tuple.Create(6, taskAssDriverLicense));

            var taskVichel = Helpers.FileHelper.SaveTruckFile(truck.FileVehicleRegistration, Helpers.PathType.Document, truck.VehicleRegistration);
            tasks.Add(Tuple.Create(7, taskVichel));

            var taskKeur = Helpers.FileHelper.SaveTruckFile(truck.FileKeurDLLAJR, Helpers.PathType.Document, truck.KeurDLLAJR);
            tasks.Add(Tuple.Create(8, taskKeur));

            var resultTask = await Task.WhenAll(tasks.Select(x => x.Item2).ToArray());
            foreach (var item in tasks)
            {
                var filename = item.Item2.Result;
                if (!string.IsNullOrEmpty(filename))
                {
                    switch (item.Item1)
                    {
                        case 1:
                            truck.DriverIDCard = filename;
                            break;
                        case 2:
                            truck.AssdriverIDCard = filename;
                            break;
                        case 3:
                            truck.DriverPhoto = filename;
                            break;
                        case 4:
                            truck.AssdriverPhoto = filename;
                            break;
                        case 5:
                            truck.DriverLicense = filename;
                            break;
                        case 6:
                            truck.AssdriverLicense = filename;
                            break;
                        case 7:
                            truck.VehicleRegistration = filename;
                            break;
                        case 8:
                            truck.KeurDLLAJR = filename;
                            break;

                    }
                }
            }

            return truck;
        }

    }
}