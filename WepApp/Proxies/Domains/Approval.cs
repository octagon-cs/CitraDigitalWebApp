using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Helpers;

namespace WebApp.Models
{
    public interface IApproval
    {
        List<KIM> GetInfoPengajuanById(int id);
        List<KIM> Checked(Pengajuan kim);
        bool Approve(Pengajuan pengajuan);
        Task<List<Pengajuan>> GetPengajuanNotApprove();
    }


    public class Approval : IApproval
    {
        private User user;

        public Approval(User user)
        {
            this.user = user;
        }

        public bool Approve(Pengajuan pengajuan)
        {
            throw new System.NotImplementedException();
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
            var pengajuanStrore = new DataStores.PengajuanDataStrore();

           return pengajuanStrore.GetPengajuanNotApproved(UserType.Approval1);
        }

        private Task<List<Pengajuan>> GetPengajuanNotApprovedByHse()
        {
            throw new NotImplementedException();
        }

        private Task<List<Pengajuan>> GetPengajuanNotApprovedByManager()
        {
            throw new NotImplementedException();
        }
    }
}