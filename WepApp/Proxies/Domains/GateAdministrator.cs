using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Services;

namespace WebApp.Proxy.Domains
{
    public interface IGateAdministrator
    {
        Task<List<Truck>> GetTrucks();
        Task<PengajuanItem> GetLastPengajuan(int truckId);
        Task<TruckIncomming> Approve(int id, List<HasilPemeriksaan> model, bool status);
        Task<Truck> TruckHistrories(int id);
    }

    public class GateAdministrator : IGateAdministrator
    {
        private DataContext _context;

        public GateAdministrator()
        {
            _context = GetServiceProvider.Instance.GetRequiredService<DataContext>();
        }

        public async Task<List<Truck>> GetTrucks()
        {
            var results = await _context.Trucks
            .Include(x => x.Kims)
            .Include(x => x.Company).AsNoTracking().ToListAsync();
            return results;
        }


        public async Task<PengajuanItem> GetLastPengajuan(int truckId)
        {
            try
            {
                var pengajuans = await _context.PengajuanItems.Where(x => x.TruckId == truckId)
                .Include(x => x.Truck).ThenInclude(x => x.Company)
                .Include(x => x.Persetujuans).ToListAsync();

                if (pengajuans == null || pengajuans.Count <= 0)
                    throw new SystemException("Pengajuan KIM Truck Belum Diajukan");
                var pengajuan = pengajuans.Last();
                if (pengajuan != null)
                {
                    pengajuan.HasilPemeriksaan = await _context.HasilPemeriksaans.Where(x => x.ItemPengajuanId == pengajuan.Id)
                    .Include(x => x.ItemPemeriksaan).ThenInclude(x => x.Pemeriksaan)
                    .AsNoTracking().ToListAsync();
                }
                return pengajuan;
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<TruckIncomming> Approve(int id, List<HasilPemeriksaan> hasil, bool status)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (hasil == null || hasil.Count <= 0)
                    throw new SystemException("Hasil Pemeriksaan Tidak Ditekuan..!");

                var itemPengajuan = _context.PengajuanItems.Where(x => x.Id == id)
                .Include(x => x.Truck).AsNoTracking().FirstOrDefault();

                if (itemPengajuan == null)
                    throw new SystemException("Data Truck Not Found..!");

                var incomming = new TruckIncomming()
                {
                    TruckId = itemPengajuan.TruckId,
                    Notes = new List<IncommingNote>(),
                    Created = DateTime.Now,
                    Status = status
                };

                foreach (var item in hasil)
                {
                    if (!item.Hasil)
                        incomming.Notes.Add(new IncommingNote
                        {
                            Note = $"Tindak Lanjut : {item.TindakLanjut} \n\r Keterangan : {item.Keterangan}",
                            CompensationDeadline = item.CompensationDeadline
                        });
                }

                _context.TruckIncommings.Add(incomming);
                var intResult = await _context.SaveChangesAsync();
                if (intResult <= 0)
                    throw new SystemException("Data Pemeriksaaan Tidak Berhasil Dibuat, Coba Ulangi Lagi !");
                await transaction.CommitAsync();
                return incomming;
            }
            catch (System.Exception ex)
            {
                await transaction.RollbackAsync();
                throw new SystemException(ex.Message);
            }
        }

        public Task<Truck> TruckHistrories(int id)
        {
            var truck = _context.Trucks.Where(x => x.Id == id)
                .Include(x => x.Kims)
                .Include(x => x.Incommings).ThenInclude(x => x.Notes)
                .AsNoTracking().FirstOrDefault();
            return Task.FromResult(truck);

        }
    }
}
