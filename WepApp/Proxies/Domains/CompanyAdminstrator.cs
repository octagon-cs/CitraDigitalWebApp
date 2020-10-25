using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.DataStores;
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
    }

    public class CompanyAdministrator : ICompanyAdministrator
    {
        public async Task<Pengajuan> AddNewPengajuanTruck(Pengajuan pengajuan)
        {
            var pengajuanStore = new PengajuanDataStrore();
            var result = await pengajuanStore.Insert(pengajuan);
            return result;
        }

        public async Task<Truck> AddNewTruck(Truck truck)
        {
            var dataStrore = new TruckDataStrore();
            var result = await dataStrore.InsertAndGetLastId(truck);
            return result;

        }

        public Task<bool> ChangeQRPPejabat(int companyId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<CompanyProfile> CreateProfile(CompanyProfile profile)
        {
            try
            {
                var dataStrore = new CompanyProfileDataStrore();
                var result = await dataStrore.InsertAndGetLastId(profile);
                if (result == null)
                    throw new System.SystemException("Create Profile Invalid..!");
                return result;

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

        public async Task<IEnumerable<Truck>> GetTrucks(int companyId)
        {
            var truckStore = new TruckDataStrore();
            var result = await truckStore.GetTrucksByCompanyId(companyId);
            return result;
        }
    }
}