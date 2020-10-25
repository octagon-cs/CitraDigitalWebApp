using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using WebApp.Models;

namespace WebApp.Helpers
{

    public static class UserExtentions
    {
        public static Task<User> GetUser(this HttpRequest req)
        {
            using (var connection = DapperContext.Connection)
            {
               var result =  (User)req.HttpContext.Items["User"];
                return Task.FromResult(result);
            }
        }


         public static async Task<CompanyProfile> GetCompany(this User user)
        {
            try
            {
                using (var connection = DapperContext.Connection)
            {
                var companies = await connection.QueryAsync("select * from companyprofile where UserId=@UserId", new {UserId=user.Id});
                if(companies!=null && companies.Count()>0)
                    return   companies.FirstOrDefault(); 
                throw new System.SystemException("You Not Have Access !");
            }
            }
            catch (System.Exception ex)
            {
                
                throw new System.SystemException(ex.Message);
            }
        }

        public static async Task<List<Role>> GetRoles(this User user)
        {
              var sql = @"SELECT
                        `roles`.`Name`
                        FROM
                        `userinroles`
                        LEFT JOIN `roles` ON `userinroles`.`RoleId` = `roles`.`Id` where userid=@userid";

            using (var connection = DapperContext.Connection)
            {
              var roles = await connection.QueryAsync<Role>(sql, new {UserId=user.Id});
              return roles.ToList();
            }
        }


    }
}