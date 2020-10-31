using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using WebApp.Helpers;

namespace WebApp.Models
{
    public class Truck : ITruck
    {
        [Key]
        public int Id { get; set; }
        public string PlateNumber { get; set; }
        public string Merk { get; set; }
        public int CarCreated { get; set; }
        public string TruckType { get; set; }
        public string DriverName { get; set; }
        public string DriverIDCard { get; set; }
        public string DriverLicense { get; set; }
        public string DriverPhoto { get; set; }

        public string AssdriverName { get; set; }
        public string AssdriverIDCard { get; set; }
        public string AssdriverLicense { get; set; }
        public string AssdriverPhoto { get; set; }
        public string KeurDLLAJR { get; set; }
        public string VehicleRegistration { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public List<KIM> Kims { get; set; } = new List<KIM>();


        [NotMapped]
        public KIM KIM
        {
            get
            {
                if (Kims == null || Kims.Count <= 0)
                    return null;
                return Kims.Last();
            }
        }


        [NotMapped]

        public FileData FileDriverPhoto { get; set; }


        [NotMapped]
        public FileData FileDriverId { get; set; }

        [NotMapped]
        public FileData FileDriverLicense { get; set; }

        [NotMapped]
        public FileData FileAssDriverPhoto { get; set; }


        [NotMapped]
        public FileData FileAssDriverId { get; set; }


        [NotMapped]
        public FileData FileAssDriverLicense { get; set; }

        [NotMapped]
        public FileData FileVehicleRegistration { get; set; }


        [NotMapped]
        public FileData FileKeurDLLAJR { get; set; }














    }

    public interface ITruck
    {
        int Id { get; set; }
        string PlateNumber { get; set; }
        string Merk { get; set; }
        int CarCreated { get; set; }
        string TruckType { get; set; }
        string DriverName { get; set; }
        string DriverIDCard { get; set; }
        string DriverLicense { get; set; }
        string DriverPhoto { get; set; }

        string AssdriverName { get; set; }
        string AssdriverIDCard { get; set; }
        string AssdriverLicense { get; set; }
        string AssdriverPhoto { get; set; }
        string KeurDLLAJR { get; set; }
        string VehicleRegistration { get; set; }
        int CompanyId { get; set; }
        Company Company { get; set; }
        List<KIM> Kims { get; set; }
        KIM KIM
        {
            get
            {
                if (Kims == null || Kims.Count <= 0)
                    return null;
                return Kims.Last();
            }
        }

    }
}