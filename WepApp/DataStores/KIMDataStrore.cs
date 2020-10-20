using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApp.Models;

namespace WebApp.DataStores{

    public class KIMDataStrore : IDataStores<KIM>
    {
       public async Task<bool> Delete(KIM t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var result = await connection.ExecuteAsync("Delete  from KIM where id = @id", t.Id);
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
        public async Task<IEnumerable<KIM>> Get()
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var results = await connection.QueryAsync<KIM>($"select * from KIM");
                    return results;
                }
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<KIM> GetById(int id)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var result = await connection.QueryAsync<KIM>($"select * from KIM where id = @id", new { id });
                    return result.FirstOrDefault();
                }
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<KIM> Insert(KIM t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var sql = $"insert into KIM(...) values(....)";
                    var result = await connection.ExecuteAsync(sql, new { });
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

        public async Task<KIM> InsertAndGetLastId(KIM t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var sql = $"insert into KIM(...) values(....); Select Last;";
                    var result = await connection.ExecuteAsync(sql, new { });
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

        public async Task<bool> Update(KIM t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var sql = $"update KIM= set ";
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