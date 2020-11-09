using System;
using System.Collections.Generic;
using WebApp.Helpers;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class TruckIncomming
    {
        [Key]
        public int Id { get; set; }

        public int TruckId { get; set; }
        
        public bool Status { get; set; }

        public DateTime Created { get; set; }

        public List<IncommingNote> Notes { get; set; }
    }


    public class  IncommingNote
    {
        [Key]
        public int Id { get; set; }

        public string Note { get; set; }

        public DateTime? CompensationDeadline{get;set;} 
    }
}