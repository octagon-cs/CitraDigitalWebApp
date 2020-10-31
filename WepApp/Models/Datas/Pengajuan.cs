using System;
using System.Collections.Generic;
using WebApp.Helpers;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Pengajuan
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public string LetterNumber { get; set; }
        public StatusPengajuan StatusPengajuan { get; set; }
        public DateTime Created { get; set; }
        public List<PengajuanItem> Items { get; set; }
    }
}