using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApp.Models;

namespace WebApp.DataStores{

    public class UserRoleDataStore : IDataStores<UserRole>
    {
       public async Task<bool> Delete(UserRole t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var result = await connection.ExecuteAsync("Delete  from UserRole where userid = @userid and roleid=@roleid", new {t.UserId, t.RoleId});
                    if (result > 0)
                        return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }

        }
        public async Task<IEnumerable<UserRole>> Get()
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var results = await connection.QueryAsync<UserRole>($"select * from UserRole");
                    return results;
                }
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<UserRole> GetById(int id)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var result = await connection.QueryAsync<UserRole>($"select * from UserRole where id=@id", new { id });
                    return result.FirstOrDefault();
                }
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

         public async Task<bool> AddUserRole(int userId,string roleName)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var roles = await connection.QueryAsync<Role>($"select * from roles where name=@name", new {name=roleName});
                    if(roles.FirstOrDefault()==null)
                        throw new SystemException($"Role {roleName} Not Exists !");
                    
                    var userInRolesExists = await connection.QueryAsync<UserRole>($"select * from userinroles where userid = @userid and RoleId=@roleid", new { userId, roleid=roles.FirstOrDefault().Id  });
                    if(userInRolesExists.FirstOrDefault()!=null)
                        throw new SystemException($"User Role {roleName}  Exists !");    

                    var sqlAddRole = "Insert into userinroles (UserId, RoleId) values (@userid, @roleid)";    
                    var result = await connection.ExecuteAsync(sqlAddRole, new UserRole{UserId=userId, RoleId=roles.FirstOrDefault().Id}); 
                    if(result>0)  
                        return true;
                    
                     throw new SystemException($"Can't Add Role {roleName}, Please Try Again ! ");
                }
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }


        public async Task<bool> RemoveUserRole(int UserId, string roleName)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                     var roles=  await  connection.QueryAsync<Role>("select * from name where name=@roleName", new {roleName});
                    if(roles==null || roles.Count()<=0)
                        throw new SystemException($"Role {roleName} Not Found !");
                    var result = await connection.ExecuteAsync("Delete  from UserRole where userid = @userid and roleid=@roleid", new {UserId,roles.FirstOrDefault().Id});
                    if (result > 0)
                        return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }

        }
        public async Task<UserRole> Insert(UserRole t)
        {
            throw new  NotImplementedException();
        }

        public async Task<UserRole> InsertAndGetLastId(UserRole t)
        {
           
           throw new NotImplementedException();
        }

        public async Task<bool> Update(UserRole t)
        {
          throw new NotImplementedException();
        }
    }
}