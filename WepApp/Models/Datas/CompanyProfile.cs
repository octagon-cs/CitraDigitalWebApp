using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApp.Helpers;

namespace WebApp.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string NPWP { get; set; }
        public string Logo { get; set; }

        [NotMapped]     
        public FileData LogoData { get; set; }
        public User User { get; set; }

    }
}