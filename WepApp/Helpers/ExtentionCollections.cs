using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp
{

    public static class UserExtentions
    {
        public static Task<User> GetUser(this HttpRequest req)
        {
            var result = (User)req.HttpContext.Items["User"];
            return Task.FromResult(result);
        }


        public static Task<Company> GetCompany(this User user)
        {
            try
            {
                var service = (DataContext)GetServiceProvider.Instance.GetService(typeof(DataContext));

                var result = service.Companies.Where(x => x.UserId == user.Id).FirstOrDefault();

                if (result == null)
                    throw new System.SystemException("You Not Have Access !");
                return Task.FromResult(result);
            }
            catch (System.Exception ex)
            {

                throw new System.SystemException(ex.Message);
            }
        }

        public static Task<List<Role>> GetRoles(this User user)
        {

            var context = (DataContext)GetServiceProvider.Instance.GetService(typeof(DataContext));
            var result = context.Users.Where(x => x.Id == user.Id).Include(x => x.UserRoles).FirstOrDefault();
            return Task.FromResult(result.UserRoles.Select(x => x.Role).ToList());

        }


    }
}