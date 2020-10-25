using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebApp.Helpers;

namespace WebApp.Models{
    public class PengajuanItem{
        public int Id{get;set;}
       public int TruckId{get;set;}

       [JsonConverter(typeof(StringEnumConverter))]
       public AttackStatus Status {get;set;}
        public int PengajuanId { get;  set; }
    }
}