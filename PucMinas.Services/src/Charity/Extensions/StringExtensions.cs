using System;
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

        public static DateTime ToTimeZonelDatetime(this DateTime value, string timeZone)
        {
            var myTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);

            var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, myTimeZone);

            return currentDateTime;
        }

        public static DateTime ToBrazilianTimeZone(this DateTime value)
        {
            var myTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

            var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, myTimeZone);

            return currentDateTime;
        }
    }
}
