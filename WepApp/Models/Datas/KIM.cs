using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Helpers;
using WepApp.Models;

namespace WebApp.Models
{
    public class KIM
    {
        [Key]
        public int Id { get; set; }

        public Truck Truck { get; set; }

        public string KimNumber { get; set; }

        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        [NotMapped]
        public ExpireStatus Expired
        {
            get
            {
                return Helpers.AutoSystem.Expired(EndDate);
            }
        }

        [NotMapped]
        public Age AgeOfKIM => new Age(this.BeginDate, DateTime.Now);


    }
}