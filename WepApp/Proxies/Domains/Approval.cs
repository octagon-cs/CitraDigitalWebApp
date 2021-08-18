using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Helpers;

namespace WebApp.Models
{
    public interface IApproval
    {
        List<KIM> GetInfoPengajuanById(int id);
        List<KIM> Checked(Pengajuan kim);
        Task<Persetujuan> Approve(int id, List<HasilPemeriksaan> hasil);
        Task<Persetujuan> Reject(int id, List<HasilPemeriksaan> hasil);
        Task<List<PengajuanItem>> GetPengajuanNotApprove();
        Task<List<HasilPemeriksaan>> GetPenilaian(int itemPengajuanId);
    }

    public class Approval : IApproval
    {
        private User user;
        private DataContext context;

        public Approval(User user)
        {
            this.user = user;
            context = GetServiceProvider.Instance.GetRequiredService<DataContext>();
        }

        public async Task<Persetujuan> Approve(int id, List<HasilPemeriksaan> hasil)
        {

            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                context.ChangeTracker.Clear();

                if (hasil == null || hasil.Count <= 0)
                    throw new SystemException("Hasil Pemeriksaan Tidak Ditekuan..!");

                var itemPengajuan = context.PengajuanItems.Where(x => x.Id == id)
                .Include(x=>x.Pengajuan)
                .Include(x => x.HasilPemeriksaan)
                .Include(x => x.Persetujuans).FirstOrDefault();

                context.Attach(itemPengajuan).State = EntityState.Modified;

                if (itemPengajuan != null && ValidateApproved(itemPengajuan))
                {

                    if (itemPengajuan.HasilPemeriksaan != null && itemPengajuan.HasilPemeriksaan.Count > 0)
                    {
                        foreach (var item in hasil)
                        {
                            var itemHasil = itemPengajuan.HasilPemeriksaan.Where(x => x.ItemPemeriksaanId == item.ItemPemeriksaanId && x.ItemPengajuanId == id).FirstOrDefault();
                            if (itemHasil == null)
                            {
                                context.Entry(item.ItemPemeriksaan).State = EntityState.Unchanged;
                                itemPengajuan.HasilPemeriksaan.Add(item);
                            }
                            else
                            {
                                context.Entry(itemHasil).CurrentValues.SetValues(item);
                            }
                        }

                    }
                    else
                        itemPengajuan.HasilPemeriksaan = hasil;

                    await context.SaveChangesAsync();
                    var persetujuan = new Persetujuan
                    {
                        User = user,
                        ApprovedBy = user.UserType,
                        ApprovedDate = DateTime.Now,
                        StatusPersetujuan = user.UserType==UserType.Company?StatusPersetujuan.Fixed: StatusPersetujuan.Approved
                    };
                    itemPengajuan.Persetujuans.Add(persetujuan); 
                    var pengajuan = context.Pengajuans.Where(x => x.Id == itemPengajuan.Pengajuan.Id)
                        .Include(x=>x.Items).ThenInclude(x=>x.Persetujuans)
                    .FirstOrDefault();
                    
                    if (persetujuan.ApprovedBy == UserType.Approval1)
                        pengajuan.StatusPengajuan = StatusPengajuan.Proccess;

                    if (persetujuan.ApprovedBy == UserType.Manager)
                        {
                            if(pengajuan.Items.Where(x=>x.Status== StatusPersetujuan.Approved).Count()== pengajuan.Items.Count()){
                                pengajuan.StatusPengajuan = StatusPengajuan.Complete;
                            }
                        }

                    await context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return persetujuan;
                }
                throw new SystemException("Item Pengajuan Not Found !");

            }
            catch (System.Exception ex)
            {
                await transaction.RollbackAsync();
                throw new SystemException(ex.Message);
            }
        }

        public async Task<Persetujuan> Reject(int id, List<HasilPemeriksaan> hasil)
        {

            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                context.ChangeTracker.Clear();
                if (hasil == null || hasil.Count <= 0)
                    throw new SystemException("Hasil Pemeriksaan Tidak Ditekuan..!");

                var itemPengajuan = context.PengajuanItems.Where(x => x.Id == id)
                .Include(x => x.Pengajuan)
                .Include(x => x.HasilPemeriksaan)
                .Include(x => x.Persetujuans).FirstOrDefault();
                context.Attach(itemPengajuan).State = EntityState.Modified;

                if (itemPengajuan != null && ValidateApproved(itemPengajuan))
                {

                    if (itemPengajuan.HasilPemeriksaan != null && itemPengajuan.HasilPemeriksaan.Count > 0)
                    {
                        foreach (var item in hasil)
                        {
                            var itemHasil = itemPengajuan.HasilPemeriksaan.Where(x => x.ItemPemeriksaanId == item.ItemPemeriksaanId && x.ItemPengajuanId == id).FirstOrDefault();
                            if (itemHasil == null)
                                itemPengajuan.HasilPemeriksaan.Add(item);
                            else
                            {
                                context.Entry(itemHasil).CurrentValues.SetValues(item);
                            }
                        }
                    }
                    else
                        itemPengajuan.HasilPemeriksaan = hasil;

                    await context.SaveChangesAsync();
                    var persetujuan = new Persetujuan
                    {
                        User=user,
                        ApprovedBy = user.UserType,
                        ApprovedDate = DateTime.Now,
                        StatusPersetujuan = StatusPersetujuan.Reject
                    };
                    itemPengajuan.Persetujuans.Add(persetujuan);
                    var pengajuan = context.Pengajuans.Where(x => x.Id == itemPengajuan.Pengajuan.Id).FirstOrDefault();
                    if (persetujuan.ApprovedBy == UserType.Approval1)
                        pengajuan.StatusPengajuan = StatusPengajuan.Proccess;

                    await context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return persetujuan;
                }
                throw new SystemException("Item Pengajuan Not Found !");

            }
            catch (System.Exception ex)
            {
                await transaction.RollbackAsync();
                throw new SystemException(ex.Message);
            }
        }

        public List<KIM> Checked(Pengajuan kim)
        {
            //get Items Pemerisaan
            throw new System.NotImplementedException();
        }

        public List<KIM> GetInfoPengajuanById(int id)
        {

            switch (this.user.UserType)
            {
                case UserType.Manager:
                    return GetAllKimByManager();

                case UserType.Approval1:
                    return GetAllKimByApproval1();

                case UserType.HSE:
                    return GetAllKimByApproval2();

                default:
                    return null;
            }
        }

        public Task<List<PengajuanItem>> GetPengajuanNotApprove()
        {
            return GetPengajuanNotApproveByUserType(this.user.UserType);
        }

      private Task<List<PengajuanItem>>  GetPengajuanNotApproveByUserType(UserType userType){
              IEnumerable<PengajuanItem> pengajuanStrore = context.PengajuanItems
            .Include(x => x.Persetujuans)
            .Include(x => x.Truck).ThenInclude(x => x.Kims)
            .Include(x => x.HasilPemeriksaan)
            .Include(x => x.Pengajuan).ThenInclude(x => x.Company).AsNoTracking().ToList();
            var results = pengajuanStrore.Where(x => x.NextApprove==userType).ToList();
            return Task.FromResult(results);
        }

        public Task<List<HasilPemeriksaan>> GetPenilaian(int pengajuanItemid)
        {
            try
            {
                var item = context.PengajuanItems.Where(x => x.Id == pengajuanItemid)
                .Include(x => x.HasilPemeriksaan).ThenInclude(x => x.ItemPemeriksaan).ThenInclude(x=>x.Pemeriksaan).AsNoTracking().FirstOrDefault();
                if (item == null)
                    throw new SystemException("Item Pengajuan Not Fornd ...!");

                if (item.HasilPemeriksaan == null || item.HasilPemeriksaan.Count <= 0)
                {
                    var datas = context.ItemPemeiksaans.AsNoTracking().ToList();
                    foreach (var itemPemeriksaan in datas)
                    {
                        var itemData = new HasilPemeriksaan { ItemPemeriksaan = itemPemeriksaan, ItemPengajuanId = pengajuanItemid, ItemPemeriksaanId = itemPemeriksaan.Id };
                        item.HasilPemeriksaan.Add(itemData);
                    }
                }
                return Task.FromResult(item.HasilPemeriksaan.OrderBy(x => x.ItemPemeriksaanId).ToList());
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }
        #region Private
        private List<KIM> GetAllKimByApproval2()
        {
            //Get All Pengajuan After Aprove by Approval 1
            throw new NotImplementedException();
        }

        private List<KIM> GetAllKimByApproval1()
        {
            //Get All Pengajuan Not Have Approval
            throw new NotImplementedException();
        }

        private List<KIM> GetAllKimByManager()
        {
            //Get All Pengajuan Have Apprval from Approval2
            throw new NotImplementedException();
        }

        private bool ValidateApproved(PengajuanItem itemPengajuan)
        {


            try
            {
                if(itemPengajuan.NextApprove == this.user.UserType){
                    return true;
                }else{

                    throw new SystemException("Bukan Saatnya Anda Melakukan Persetujuan !");
                }
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }

        }

        #endregion

    }
}