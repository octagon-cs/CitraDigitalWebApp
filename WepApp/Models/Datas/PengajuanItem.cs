using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp.Helpers;

namespace WebApp.Models
{
    public class PengajuanItem
    {
        [Key]
        public int Id { get; set; }
        public int TruckId { get; set; }

        public AttackStatus AttackStatus { get; set; }
        public int PengajuanId { get; set; }

        public List<Persetujuan> Persetujuans { get; set; } = new List<Persetujuan>();
    }
}