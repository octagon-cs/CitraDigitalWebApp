using System;
using System.Collections.Generic;
using Dapper;
using WebApp.Models;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Helpers;

namespace WebApp.DataStores
{
    public class UserDataStore : IDataStores<User>
    {
        public async Task<bool> Delete(User t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var result = await connection.ExecuteAsync("Delete  from users where id = @id", t.Id);
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
        public async Task<IEnumerable<User>> Get()
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var results = await connection.QueryAsync<User>($"select * from users");
                    return results;
                }
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<User> GetById(int id)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var result = await connection.QueryAsync<User>($"select * from users where id = @id", new { id });
                   var user = result.FirstOrDefault();
                   if(user!=null)
                    user.Roles = await user.GetRoles();
                    return user;
                }
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<User> Insert(User t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    t.Status=true;
                    var sql = @"Insert into users(UserName, Password, FirstName,LastName, Status)
                                             values(@UserName, @Password, @FirstName, @LastName, @Status)";
                    var result = await connection.ExecuteAsync(sql, t);
                    
                    {if (result >= 0)
                        t.Id = result;
                        return t;
                    }

                    throw new SystemException("Add New User Invalid ...!");
                }
            }
            catch (System.Exception ex)
            {

                throw new SystemException(ex.Message);
            }
        }

        public async Task<User> InsertAndGetLastId(User t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    t.Status=true;
                     var sql = @"Insert into users(UserName, Password, FirstName,LastName, Status)
                                             values(@UserName, @Password, @FirstName, @LastName, @Status); SELECT LAST_INSERT_ID();";
                    var result =Convert.ToInt32(await connection.ExecuteScalarAsync(sql, t));
                    if (result > 0)
                    {
                        t.Id = result;
                        return t;
                    }
                    throw new SystemException("Add New User Invalid ... !");
                }
            }
            catch (System.Exception ex)
            {

                throw new SystemException(ex.Message);
            }
        }

        public async Task<bool> Update(User t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var sql = $"update user set ";
                    var result = await connection.ExecuteAsync(sql, new { });
                    if (result > 0)
                        return true;
                    return false;
                }
            }
            catch (System.Exception ex)
            {

                throw new SystemException(ex.Message);
            }
        }
    }
}