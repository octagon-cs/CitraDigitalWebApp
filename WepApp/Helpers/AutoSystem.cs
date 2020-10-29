using System;

namespace WebApp.Helpers
{
    public class AutoSystem
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
}