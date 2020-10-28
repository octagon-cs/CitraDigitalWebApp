using System.ComponentModel.DataAnnotations;
namespace WebApp.Models
{
    public class ItemPemeriksaan
    {
        [Key]
        public int Id { get; set; }

        public string Kelengkapan { get; set; }
        public string Penjelasan { get; set; }
    }
}