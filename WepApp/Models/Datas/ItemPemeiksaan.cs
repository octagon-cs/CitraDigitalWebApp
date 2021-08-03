using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Models
{
    public class ItemPemeriksaan
    {
        [Key]
        public int Id { get; set; }
        public string Kelengkapan { get; set; }
        public string Penjelasan { get; set; }
        public JenisPemeriksaan JenisPemeriksaan { get; set; }
        public Pemeriksaan Pemeriksaan { get; set; }
    }




    public enum JenisPemeriksaan
    {
        Kelengakapan, Dokumen
    }
}