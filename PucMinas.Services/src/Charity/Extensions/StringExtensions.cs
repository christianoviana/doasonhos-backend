using System.Security.Cryptography;
using System.Text;

namespace PucMinas.Services.Charity.Extensions
{
    public static class StringExtensions
    {
        public static string ToSHA512(this string value)
        {
            using (SHA512 shaM = new SHA512Managed())
            {                
                var encript = shaM.ComputeHash(Encoding.UTF8.GetBytes(value));

                var hash = new StringBuilder();
           
                foreach (var _byte in encript)
                    hash.Append(_byte.ToString("X2"));

                return hash.ToString();
            }
        }
    }
}
