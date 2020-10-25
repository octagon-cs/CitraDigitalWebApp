using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.DataStores
{

    public class PengajuanDataStrore : IDataStores<Pengajuan>
    {
        public async Task<bool> Delete(Pengajuan t)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Pengajuan>> Get()
        {
            throw new NotImplementedException();
        }

        public async Task<Pengajuan> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Pengajuan> Insert(Pengajuan t)
        {
            MySqlTransaction trans = null;
            using (var connection = DapperContext.Connection)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();
                    trans = connection.BeginTransaction();
                    var sql = $"insert into Pengajuan(letternumber, status, created) values(@letternumber, @status, @created); select Last_Insert_Id()";
                    var result = Convert.ToInt32(await connection.ExecuteScalarAsync(sql, t));
                    if (result >= 0)
                    {
                        t.Id = result;
                        foreach (var item in t.Items)
                        {
                            item.PengajuanId = t.Id;
                            var sqlItem = $"insert into PengajuanItem (pengajuanId,  truckid, status) values (@pengajuanId, @truckid, @status); select Last_Insert_Id()";
                            var itemResult = Convert.ToInt32(await connection.ExecuteScalarAsync(sqlItem, item));
                            if (itemResult > 0)
                            {
                                item.Id = itemResult;
                            }else
                            throw new SystemException("Pengajuan Not Created !");
                        }
                        trans.Commit();
                        return t;
                    }
                    return null;
                }
                catch (System.Exception ex)
                {
                    trans.Rollback();
                    throw new SystemException(ex.Message);
                }
            }
        }



        public async Task<Pengajuan> InsertAndGetLastId(Pengajuan t)
        {
            throw new NotImplementedException();
        }

        internal Task<List<Pengajuan>> GetPengajuanNotApproved(UserType approval1)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Pengajuan t)
        {
            throw new NotImplementedException();
        }
    }
}