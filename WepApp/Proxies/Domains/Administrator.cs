using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApp.DataStores;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Proxy.Domains
{


    public interface IAdministrator
    {
        Task<User> CreateUser(User user);
        Task AddUserRole(int userId, string roleName);
        Task AddNewItemPemeriksaaan(string item);
        Task<List<KIM>> GetAllKIMNotYetApproved();

        Task<KIM> CreateNewKIM(KIM kim);

        Task<KIM> GetKIMById(int kimId);

        Task PrintKIM(KIM kim);

        Task<List<KIM>> GetAllKIM();

    }


    public class Administrator : IAdministrator
    {
        private IUserService _userService;

        public Administrator(IUserService userService)
        {
            _userService = userService;
        }
        public Task AddNewItemPemeriksaaan(string item)
        {
            throw new System.NotImplementedException();
        }

        public async Task AddUserRole(int userId, string roleName)
        {
            try
            {
                var userInRoles = new UserRoleDataStore();
                var userRole = await userInRoles.AddUserRole(userId, roleName);
                if (!userRole)
                    throw new SystemException("Role {roleName} Exists !");

            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }

        }

        public Task<KIM> CreateNewKIM(KIM kim)
        {
            throw new System.NotImplementedException();
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

        public Task PrintKIM(KIM kim)
        {
            throw new System.NotImplementedException();
        }
    }
}