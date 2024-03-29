using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WebApp.Helpers;

namespace WebApp.Models
{
    public class PengajuanItem
    {
        [Key]
        public int Id { get; set; }
        public Truck Truck { get; set; }

        public AttackStatus AttackStatus { get; set; }

        public Pengajuan Pengajuan { get; set; }

        public List<Persetujuan> Persetujuans { get; set; } = new List<Persetujuan>();
        public List<HasilPemeriksaan> HasilPemeriksaan { get; set; } = new List<HasilPemeriksaan>();

        [NotMapped]
        public StatusPersetujuan Status
        {
            get
            {

                if (Persetujuans == null || Persetujuans.Count <= 0)
                    return StatusPersetujuan.Proccess;

                var lastApproval = Persetujuans.Last();
                return lastApproval.StatusPersetujuan;
            }
        }


        [NotMapped]
        public UserType NextApprove
        {
            get
            {

                if (Persetujuans == null || Persetujuans.Count <= 0)
                    return UserType.Administrator;

                var lastPersetujuan = Persetujuans.Last();

                if (lastPersetujuan.StatusPersetujuan == StatusPersetujuan.Reject)
                    return UserType.Company;

                if (lastPersetujuan.ApprovedBy == UserType.Administrator)
                    return UserType.Approval1;

                if (lastPersetujuan.ApprovedBy == UserType.Approval1)
                    return UserType.HSE;

                if (lastPersetujuan.ApprovedBy == UserType.HSE)
                    return UserType.Manager;

                if (lastPersetujuan.ApprovedBy == UserType.Manager)
                    return UserType.Administrator;

                if (lastPersetujuan.ApprovedBy == UserType.Company)
                {
                    var index = Persetujuans.IndexOf(lastPersetujuan);
                    var lasApproved = Persetujuans[index - 1];
                    return lasApproved.ApprovedBy;
                }
                return UserType.None;

            }
        }
    }
}