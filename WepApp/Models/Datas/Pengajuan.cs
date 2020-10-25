using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebApp.Helpers;

namespace WebApp.Models{
    public class Pengajuan{
        public int Id{get;set;}
        public string LetterNumber{get;set;}

        [JsonConverter(typeof(StringEnumConverter))]
        public StatusPengajuan Status{get;set;}
        public DateTime Created{get;set;}
        public IEnumerable<PengajuanItem> Items{get;set;}
    }
}