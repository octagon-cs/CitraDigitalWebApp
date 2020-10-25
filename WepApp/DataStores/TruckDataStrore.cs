using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApp.Models;

namespace WebApp.DataStores{

    public class TruckDataStrore : IDataStores<Truck>
    {
       public async Task<bool> Delete(Truck t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var result = await connection.ExecuteAsync("Delete  from Truck where id = @id", t.Id);
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
        public async Task<IEnumerable<Truck>> Get()
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var results = await connection.QueryAsync<Truck>($"select * from Truck");
                    return results;
                }
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<Truck> GetById(int id)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var result = await connection.QueryAsync<Truck>($"select * from Truck where id = @id", new { id });
                    return result.FirstOrDefault();
                }
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<Truck> Insert(Truck t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var sql = $"insert into Truck(...) values(....)";
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

        internal async  Task<IEnumerable<Truck>> GetTrucksByCompanyId(int id)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var sql = $"Select & from truck where companyId=@id";
                    var result = await connection.QueryAsync<Truck>(sql, new {id=id });
                    if (result !=null && result.Count()>0)
                        return result;
                    return null;
                }
            }
            catch (System.Exception ex)
            {

                throw new SystemException(ex.Message);
            }
        }

        public async Task<Truck> InsertAndGetLastId(Truck t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var sql = @"insert into truck(platenumber, Merk, CarCreated, TruckType, DriverName, DriverPhoto, DriverIdCard,DriverLicense,
                    AssdriverName, assdriverphoto, assdriveridcard, assdriverlicense, VehicleRegistration, KeurDLLAJR, CompanyId ) 
                    values(@platenumber, @Merk, @CarCreated, @TruckType, @DriverName, @DriverPhoto, @DriverIdCard, @DriverLicense,
                    @AssdriverName, @Assdriverphoto, @Assdriveridcard, @Assdriverlicense, @VehicleRegistration, @KeurDLLAJR, @CompanyId); SELECT LAST_INSERT_ID();";
                    var result = Convert.ToInt32(await connection.ExecuteScalarAsync(sql, t));
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

        public async Task<bool> Update(Truck t)
        {
            try
            {
                using (var connection = DapperContext.Connection)
                {
                    var sql = $"update Truck= set ";
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