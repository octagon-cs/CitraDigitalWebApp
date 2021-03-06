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

                if (hasil == null || hasil.Count <= 0)
                    throw new SystemException("Hasil Pemeriksaan Tidak Ditekuan..!");

                var itemPengajuan = context.PengajuanItems.Where(x => x.Id == id)
                .Include(x => x.HasilPemeriksaan)
                .Include(x => x.Persetujuans).FirstOrDefault();


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
                        PengajuanItemId = id,
                        UserId = user.Id,
                        ApprovedBy = user.UserType,
                        ApprovedDate = DateTime.Now,
                        StatusPersetujuan = StatusPersetujuan.Approved
                    };
                    context.Persetujuans.Add(persetujuan);
                    var pengajuan = context.Pengajuans.Where(x => x.Id == itemPengajuan.PengajuanId).FirstOrDefault();
                    if (persetujuan.ApprovedBy == UserType.Approval1)
                        pengajuan.StatusPengajuan = StatusPengajuan.Proccess;

                    if (persetujuan.ApprovedBy == UserType.Manager)
                        pengajuan.StatusPengajuan = StatusPengajuan.Complete;

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

                if (hasil == null || hasil.Count <= 0)
                    throw new SystemException("Hasil Pemeriksaan Tidak Ditekuan..!");

                var itemPengajuan = context.PengajuanItems.Where(x => x.Id == id)
                .Include(x => x.HasilPemeriksaan)
                .Include(x => x.Persetujuans).FirstOrDefault();


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
                        PengajuanItemId = id,
                        UserId = user.Id,
                        ApprovedBy = user.UserType,
                        ApprovedDate = DateTime.Now,
                        StatusPersetujuan = StatusPersetujuan.Reject
                    };

                    context.Persetujuans.Add(persetujuan);
                    var pengajuan = context.Pengajuans.Where(x => x.Id == itemPengajuan.PengajuanId).FirstOrDefault();
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
            switch (this.user.UserType)
            {
                case UserType.Manager:
                    return GetPengajuanNotApprovedByManager();

                case UserType.Approval1:
                    return GetPengajuanNotApprovedByApproval1();

                case UserType.HSE:
                    return GetPengajuanNotApprovedByHse();

                default:
                    return null;
            }
        }

        public Task<List<HasilPemeriksaan>> GetPenilaian(int pengajuanItemid)
        {
            try
            {
                var item = context.PengajuanItems.Where(x => x.Id == pengajuanItemid)
                .Include(x => x.HasilPemeriksaan)
                .ThenInclude(x => x.ItemPemeriksaan)
                .Include(x => x.HasilPemeriksaan)
                .ThenInclude(x => x.ItemPemeriksaan.Pemeriksaan).AsNoTracking()

                .FirstOrDefault();
                if (item == null)
                    throw new SystemException("Item Pengajuan Not Fornd ...!");

                if (item.HasilPemeriksaan == null || item.HasilPemeriksaan.Count <= 0)
                {
                    var datas = context.ItemPemeiksaans.ToList();
                    foreach (var itemPemeriksaan in datas)
                    {
                        item.HasilPemeriksaan.Add(new HasilPemeriksaan { ItemPengajuanId = pengajuanItemid, ItemPemeriksaanId = itemPemeriksaan.Id });
                    }
                }
                context.SaveChangesAsync();
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

        private Task<List<PengajuanItem>> GetPengajuanNotApprovedByApproval1()
        {
            var pengajuanStrore = context.PengajuanItems.Where(x => x.Persetujuans == null || x.Persetujuans.Count <= 0)
            .Include(x => x.Persetujuans)
            .Include(x => x.Truck).ThenInclude(x => x.Kims)
            .Include(x => x.HasilPemeriksaan)
            .Include(x => x.Pengajuan).ThenInclude(x => x.Company);
            return pengajuanStrore.Where(x => x.Persetujuans == null || x.Persetujuans.Count <= 0).ToListAsync();
        }

        private Task<List<PengajuanItem>> GetPengajuanNotApprovedByHse()
        {
            var pengajuanStrore = context.PengajuanItems.Where(x => x.Persetujuans != null || x.Persetujuans.Count == 1)
           .Include(x => x.Persetujuans)
           .Include(x => x.Truck).ThenInclude(x => x.Kims)
           .Include(x => x.HasilPemeriksaan)
           .Include(x => x.Pengajuan).ThenInclude(x => x.Company);
            return pengajuanStrore.Where(x => x.Persetujuans.Count == 1 ).ToListAsync();
        }

        private Task<List<PengajuanItem>> GetPengajuanNotApprovedByManager()
        {
            var pengajuanStrore = context.PengajuanItems.Where(x => x.Persetujuans != null || x.Persetujuans.Count == 2)
            .Include(x => x.Persetujuans)
            .Include(x => x.Truck).ThenInclude(x => x.Kims)
            .Include(x => x.HasilPemeriksaan)
            .Include(x => x.Pengajuan).ThenInclude(x => x.Company);
            return pengajuanStrore.Where(x => x.Persetujuans.Count == 2 ).ToListAsync();
        }

        private bool ValidateApproved(PengajuanItem itemPengajuan)
        {
            var persetujuanExists = itemPengajuan.Persetujuans.Where(x => x.ApprovedBy == this.user.UserType).FirstOrDefault();
            if (persetujuanExists != null)
                throw new SystemException("Approved Is Exists...!");
            try
            {
                switch (this.user.UserType)
                {
                    case UserType.Manager:
                        var hse = itemPengajuan.Persetujuans.Where(x => x.ApprovedBy == UserType.HSE).FirstOrDefault();
                        if (hse == null)
                        {
                            throw new SystemException("HSE Not Yet Approve");
                        }

                        return true;

                    case UserType.Approval1:
                        return true;

                    case UserType.HSE:
                        var approval1 = itemPengajuan.Persetujuans.Where(x => x.ApprovedBy == UserType.Approval1).FirstOrDefault();
                        if (approval1 == null)
                        {
                            throw new SystemException("Approval1 Not Yet Approve");
                        }

                        return true;

                }


                return false;


            }
            catch (System.Exception ex)
            {

                throw new SystemException(ex.Message);
            }

        }

        #endregion

    }
}