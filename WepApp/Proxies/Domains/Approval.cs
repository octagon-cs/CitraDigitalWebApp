using System;
using System.Collections.Generic;
using WebApp.Helpers;

namespace WebApp.Models
{
    public interface IApproval
    {
        List<KIM> GetInfoPengajuanById(int id);
        List<KIM> Checked(Pengajuan kim);
        bool Approve(Pengajuan pengajuan);
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

                case UserType.Approval2:
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

       
    }
}