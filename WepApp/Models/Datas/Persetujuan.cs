using System;
using System.ComponentModel.DataAnnotations;
using WebApp.Helpers;

namespace WebApp.Models
{
    public class Persetujuan
    {
        [Key]

        public int Id { get; set; }

        public int PengajuanItemId { get; set; }

        public int UserId { get; set; }
        public StatusPersetujuan StatusPersetujuan { get; set; }

        public UserType ApprovedBy { get; set; }

        public DateTime ApprovedDate { get; set; }

    }
}