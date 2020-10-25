using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebApp.Models;

namespace WebApp.DataStores
{

    public class ItemPemeriksaanDataStrore : IDataStores<ItemPemeriksaan>
    {
        public async Task<bool> Delete(ItemPemeriksaan t)
        {
            throw new NotImplementedException();

        }
        public async Task<IEnumerable<ItemPemeriksaan>> Get()
        {
            throw new NotImplementedException();
        }

        public async Task<ItemPemeriksaan> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ItemPemeriksaan> Insert(ItemPemeriksaan t)
        {
            using (var connection = DapperContext.Connection)
            {
                try
                {
                    var sql = $"insert into ItemPemeriksaan (kelengkapan, penjelasan) values (@kelengkapan, @penjelasan); Select Last_Insert_Id()";
                    var id = Convert.ToInt32(await connection.ExecuteAsync(sql, t));
                    if (id > 0)
                    {
                        t.Id = id;
                        return t;
                    }
                    throw new SystemException("Item Pemeriksaan Not Saved !");
                }
                catch (System.Exception ex)
                {
                    throw new SystemException(ex.Message);
                }
            }
        }

        public async Task<ItemPemeriksaan> InsertAndGetLastId(ItemPemeriksaan t)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(ItemPemeriksaan t)
        {
            throw new NotImplementedException();
        }
    }
}