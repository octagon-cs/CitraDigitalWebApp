using System;

namespace WebApp.Helpers
{
    public class AgeHelper
    {
        internal static ExpireStatus Expired(DateTime endDate)
        {
            var sisa = endDate.Subtract(DateTime.Now);
            if (sisa.Days > 0 && sisa.Days <= 7)
                return ExpireStatus.WillExpire;
            if (sisa.Days <= 0)
                return ExpireStatus.Expire;

            return ExpireStatus.None;
        }
    }



    public class Age{

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public Age(DateTime? dateOfBird)
        {
            if (dateOfBird != null)
            {
                DateTime selisih = new DateTime(DateTime.Now.Subtract(dateOfBird.Value).Ticks);
                Year = selisih.Year;
                Month = selisih.Month;
                Day = selisih.Day;
            }
        }
    }
}