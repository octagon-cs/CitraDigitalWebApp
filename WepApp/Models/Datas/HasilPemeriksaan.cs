using System.ComponentModel.DataAnnotations;
using System;
namespace WebApp.Models
{
    public class HasilPemeriksaan
    {
        [Key]
        public int Id { get; set; }
        public int ItemPengajuanId { get; set; }
        public int ItemPemeriksaanId { get; set; }
        public bool Hasil { get; set; }
        public string TindakLanjut { get; set; }
        public string Keterangan { get; set; }
        public DateTime? CompensationDeadline { get; set; }
        public ItemPemeriksaan ItemPemeriksaan { get; set; }
    }
}