using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public class CompanyProfile
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string NPWP { get; set; }
        public string Logo { get; set; }

        [JsonIgnore]
        public byte[] LogoData { get; set; }
        public int UserId { get; set; }

    }
}