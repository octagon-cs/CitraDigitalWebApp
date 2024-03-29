using System.Security.Cryptography;
using System.Text;

namespace WebApp.Helpers
{
    public class MD5Hash
    {
        public static string ToMD5Hash(string input)
        {
           try
           {
                StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
           }
           catch (System.Exception)
           {
               return string.Empty;
           }
        }
    }
}