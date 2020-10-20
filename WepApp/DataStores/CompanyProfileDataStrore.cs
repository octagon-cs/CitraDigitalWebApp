using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApp.Models;

namespace WebApp.DataStores{

    public class CompanyProfileDataStrore : IDataStores<CompanyProfile>
    {
       public async Task<bool> Delete(CompanyProfile t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var result = await connection.ExecuteAsync("Delete  from CompanyProfile where id = @id", t.Id);
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
        public async Task<IEnumerable<CompanyProfile>> Get()
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var results = await connection.QueryAsync<CompanyProfile>($"select * from CompanyProfile");
                    return results;
                }
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<CompanyProfile> GetById(int id)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var result = await connection.QueryAsync<CompanyProfile>($"select * from CompanyProfile where id = @id", new { id });
                    return result.FirstOrDefault();
                }
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<CompanyProfile> Insert(CompanyProfile t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var sql = $"insert into CompanyProfile(...) values(....)";
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

        public async Task<CompanyProfile> InsertAndGetLastId(CompanyProfile t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var sql = $"insert into CompanyProfile(name, address, email,npwp, logo) values(@name, @address, @email,@npwp, @logo); SELECT LAST_INSERT_ID();";
                    var result = Convert.ToInt32(await connection.ExecuteScalarAsync(sql, t));
                    if (result > 0)
                    {
                        t.Id = result;
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

        public async Task<bool> Update(CompanyProfile t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var sql = $"update CompanyProfile= set ";
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