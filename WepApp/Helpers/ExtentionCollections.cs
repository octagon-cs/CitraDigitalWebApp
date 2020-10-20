using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApp.Models;

namespace WebApp.Helpers
{

    public static class UserExtentions
    {
        public static Task<User> GetUser(this System.Security.Claims.ClaimsPrincipal claim)
        {
            using (var connection = DapperContext.Connection)
            {
                var result = connection.Query<User>("Select * from Users where id=@id", new { Id = claim.Identity.Name });
                return Task.FromResult(result.FirstOrDefault());
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