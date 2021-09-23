using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
        public int Capacity { get; set; }
        public string TruckType { get; set; }
        public string TruckPhoto { get; set; }
        public string DriverName { get; set; }
        public DateTime? DriverDateOfBirth { get; set; }
        public DateTime? AssDriverDateOfBirth { get; set; }
        public int TahunPembuatan { get; set; }

        [NotMapped]
        public DataDocument DriverIDCard { get; set; }


        [NotMapped]
        public Age DriverAge => new Age(DriverDateOfBirth);

        [NotMapped]
        public Age AssDriverAge => new Age(AssDriverDateOfBirth);


        [JsonIgnore]
        [Column("DriverIDCard")]
        public string _DriverIDCard
        {
            get
            {
                return DriverIDCard == null ? string.Empty : JsonConvert.SerializeObject(DriverIDCard);
            }
            set
            {
                DriverIDCard = JsonConvert.DeserializeObject<DataDocument>(value);
            }
        }



        [NotMapped]
        public DataDocument DriverLicense { get; set; }

        [JsonIgnore]
        [Column("DriverLicense")]
        public string _DriverLicense
        {
            get
            {
                return DriverLicense == null ? string.Empty : JsonConvert.SerializeObject(DriverLicense);
            }
            set
            {
                DriverLicense = JsonConvert.DeserializeObject<DataDocument>(value);
            }
        }


        public string DriverPhoto { get; set; }

        public string AssdriverName { get; set; }

        [NotMapped]
        public DataDocument AssdriverIDCard { get; set; }


        [JsonIgnore]
        [Column("AssdriverIDCard")]
        public string _AssdriverIDCard
        {
            get
            {
                return AssdriverIDCard == null ? string.Empty : JsonConvert.SerializeObject(AssdriverIDCard);
            }
            set
            {
                AssdriverIDCard = JsonConvert.DeserializeObject<DataDocument>(value);
            }
        }

        [NotMapped]
        public DataDocument AssdriverLicense { get; set; }

        [JsonIgnore]
        [Column("AssdriverLicense")]
        public string _AssdriverLicense
        {
            get
            {
                return AssdriverLicense == null ? string.Empty : JsonConvert.SerializeObject(AssdriverLicense);
            }
            set
            {
                AssdriverLicense = value == null ? null : JsonConvert.DeserializeObject<DataDocument>(value);
            }
        }

        public string AssdriverPhoto { get; set; }

        [NotMapped]
        public DataDocument KeurDLLAJR { get; set; }


        [JsonIgnore]
        [Column("KeurDLLAJR")]
        public string _KeurDLLAJR
        {
            get
            {
                return KeurDLLAJR == null ? string.Empty : JsonConvert.SerializeObject(KeurDLLAJR);
            }
            set
            {
                KeurDLLAJR = value == null ? null : JsonConvert.DeserializeObject<DataDocument>(value);
            }
        }


        [NotMapped]
        public DataDocument VehicleRegistration { get; set; }
        [JsonIgnore]
        [Column("VehicleRegistration")]
        public string _VehicleRegistration
        {
            get
            {
                return VehicleRegistration == null ? string.Empty : JsonConvert.SerializeObject(VehicleRegistration);
            }
            set
            {
                VehicleRegistration = value == null ? null : JsonConvert.DeserializeObject<DataDocument>(value);
            }
        }


        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public List<TruckIncomming> Incommings { get; set; }
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
        public TruckIncomming LastIncomming
        {
            get
            {
                if (Incommings == null || Incommings.Count <= 0)
                    return null;
                return Incommings.Last();
            }
        }


        [NotMapped]

        public FileData FileTruckPhoto { get; set; }

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


    public class DataDocument
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime? Berlaku { get; set; }
        public DateTime? Hingga { get; set; }
        public string Document { get; set; }
    }


    public class ITruck
    {

        int Id { get; set; }
        string PlateNumber { get; set; }
        string Merk { get; set; }
        int CarCreated { get; set; }
        int Capacity { get; set; }
        string TruckType { get; set; }
        string TruckPhoto { get; set; }
        string DriverName { get; set; }

        DataDocument DriverIDCard { get; set; }


        DataDocument DriverLicense { get; set; }


        string DriverPhoto { get; set; }

        string AssdriverName { get; set; }

        DataDocument AssdriverIDCard { get; set; }


        DataDocument AssdriverLicense { get; set; }


        string AssdriverPhoto { get; set; }

        DataDocument KeurDLLAJR { get; set; }

        DataDocument VehicleRegistration { get; set; }

        int CompanyId { get; set; }
        Company Company { get; set; }

        List<KIM> Kims { get; set; } = new List<KIM>();

        KIM KIM
        {
            get
            {
                if (Kims == null || Kims.Count <= 0)
                    return null;
                return Kims.Last();
            }
        }

        List<TruckIncomming> Incommings { get; set; }

        TruckIncomming LastIncomming
        {
            get
            {
                if (Incommings == null || Incommings.Count <= 0)
                    return null;
                return Incommings.Last();
            }
        }

    }
}