using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Helpers;

namespace WebApp.Models
{
    public class Truck
    {
        [Key]
        public int Id { get; set; }
        public string PlateNumber { get; set; }
        public string Merk { get; set; }
        public int CarCreated { get; set; }
        public string TruckType { get; set; }
        public string DriverName { get; set; }
        public string DriverPhoto { get; set; }

        public string DriverIDCard { get; set; }
        public string DriverLicense { get; set; }
        public string AssdriverName { get; set; }

        public string AssdriverPhoto { get; set; }
        public string AssdriverIDCard { get; set; }
        public string AssdriverLicense { get; set; }
        public string VehicleRegistration { get; set; }

        public string KeurDLLAJR { get; set; }

        public int CompanyId { get; set; }


        [NotMapped]
        public FileData FileDriverLicense { get; set; }

        [NotMapped]
        public FileData FileAssDriverLicense { get; set; }

        [NotMapped]
        public FileData FileDriverId { get; set; }

        [NotMapped]
        public FileData FileAssDriverId { get; set; }

        [NotMapped]
        public FileData FileDriverPhoto { get; set; }

        [NotMapped]
        public FileData FileAssDriverPhoto { get; set; }

    }
}