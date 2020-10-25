using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApp.Models;

namespace WebApp.DataStores
{

    public class RoleDataStrore : IDataStores<Role>
    {
        public async Task<bool> Delete(Role t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var result = await connection.ExecuteAsync("Delete  from Roles where id = @id", t.Id);
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
        public async Task<IEnumerable<Role>> Get()
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var results = await connection.QueryAsync<Role>($"select * from Roles");
                    return results;
                }
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<Role> GetById(int id)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var result = await connection.QueryAsync<Role>($"select * from Roles where id = @id", new { id });
                    return result.FirstOrDefault();
                }
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<Role> Insert(Role t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var sql = $"insert into Roles(name) values(@name)";
                    var result = await connection.ExecuteAsync(sql, t);
                    if (result >= 0)
                    {
                        t.Id = t.Id;
                        return t;
                    }

                    return null;
                }
            }
            catch (System.Exception ex)
            {

                throw new SystemException(ex.Message);
            }
        }

        public async Task<Role> InsertAndGetLastId(Role t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var sql = $"insert into Roles(name) values(@name);  SELECT LAST_INSERT_ID();";
                    var result = await connection.ExecuteAsync(sql, t);
                    if (result > 0)
                    {
                        t.Id = t.Id;
                        return t;
                    }
                    return null;
                }
            }
            catch (System.Exception ex)
            {

                throw new SystemException(ex.Message);
            }
        }

        public async Task<bool> Update(Role t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var sql = $"update Roles= set name=@name ";
                    var result = await connection.ExecuteAsync(sql, t);
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

        public async Task<Role> GetByName(string name)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var result = await connection.QueryAsync<Role>("select * from roles where name = @name", new { name = name });
                    if (result != null)
                        return result.FirstOrDefault();
                    return null;

                }
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

    }
}