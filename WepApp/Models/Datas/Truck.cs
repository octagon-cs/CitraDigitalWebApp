using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
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


        [JsonIgnore]
        [NotMapped]
        public FileData FileDriverPhoto { get; set; }


        [JsonIgnore]
        [NotMapped]
        public FileData FileDriverId { get; set; }

        [JsonIgnore]
        [NotMapped]
        public FileData FileDriverLicense { get; set; }

        [JsonIgnore]
        [NotMapped]
        public FileData FileAssDriverPhoto { get; set; }


        [JsonIgnore]
        [NotMapped]
        public FileData FileAssDriverId { get; set; }


        [JsonIgnore]
        [NotMapped]
        public FileData FileAssDriverLicense { get; set; }

        [JsonIgnore]
        [NotMapped]
        public FileData FileVehicleRegistration { get; set; }


        [JsonIgnore]
        [NotMapped]
        public FileData FileKeurDLLAJR { get; set; }














    }
}