using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Proxy.Domains
{
    public interface ICompanyAdministrator
    {
        Task<CompanyProfile> CreateProfile(CompanyProfile model);

        Task<Truck> AddNewTruck(Truck truck);

        Task<Pengajuan> AddNewPengajuanTruck(Truck truck);

        Task<IEnumerable<KIM>> GetAllKim(int companyId);

        Task<bool> ChangeQRPPejabat(int companyId);
    }

    public class CompanyAdministrator : ICompanyAdministrator
    {
        public Task<Pengajuan> AddNewPengajuanTruck(Truck truck)
        {
            throw new System.NotImplementedException();
        }

        public Task<Truck> AddNewTruck(Truck truck)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ChangeQRPPejabat(int companyId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<CompanyProfile> CreateProfile(CompanyProfile profile)
        {
            try
            {
                var dataStrore = new DataStores.CompanyProfileDataStrore();
                var result = await dataStrore.InsertAndGetLastId(profile);
                if(result==null)
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
    }
}