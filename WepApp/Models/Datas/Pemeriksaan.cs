using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Models
{
    public class Pemeriksaan
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ItemPemeriksaan> Items { get; set; }
    }
}