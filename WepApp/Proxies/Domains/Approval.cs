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
        Task<Persetujuan> Approve(Persetujuan pengajuan);
        Task<List<Pengajuan>> GetPengajuanNotApprove();
        Task GetPenialain(int id);
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

        public async Task<Persetujuan> Approve(Persetujuan persetujuan)
        {
            try
            {
                var itemPengajuan = context.PengajuanItem.Where(x => x.Id == persetujuan.PengajuanItemId)
                .Include(x => x.Persetujuans).FirstOrDefault();
                if (itemPengajuan != null && ValidateApproved(itemPengajuan))
                {
                    persetujuan.UserId = this.user.Id;
                    persetujuan.ApprovedBy = user.UserType;
                    persetujuan.ApprovedDate = DateTime.Now;
                    context.Persetujuans.Add(persetujuan);
                    await context.SaveChangesAsync();
                    return persetujuan;

                }
                throw new SystemException("Item Pengajuan Not Found !");

            }
            catch (System.Exception ex)
            {
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

        public Task<List<Pengajuan>> GetPengajuanNotApprove()
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

        private Task<List<Pengajuan>> GetPengajuanNotApprovedByApproval1()
        {
            var pengajuanStrore = context.Pengajuans.Include(x => x.Items).ThenInclude(x => x.Persetujuans);

            foreach (var item in pengajuanStrore)
            {
                var trucks = item.Items.Where(x => x.Persetujuans == null || x.Persetujuans.Count <= 0);
                item.Items = trucks.ToList();
            }

            return pengajuanStrore.Where(x => x.Items.Count > 0).ToListAsync();
        }

        private Task<List<Pengajuan>> GetPengajuanNotApprovedByHse()
        {
            throw new NotImplementedException();
        }

        private Task<List<Pengajuan>> GetPengajuanNotApprovedByManager()
        {
            throw new NotImplementedException();
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

        public Task GetPenialain(int id)
        {


            var item = context.PengajuanItem.Where(x => x.Id == id).Include(x => x.HasilPemeriksaan).FirstOrDefault();


        }
    }
}