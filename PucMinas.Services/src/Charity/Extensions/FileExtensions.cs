using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Extensions
{
    public static class FileExtensions
    {
        public async static Task<byte[]> ToBytes(this IFormFile file)
        {
            byte[] image = { };

            // Saving Image on Server
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    image = ms.ToArray();
                }
            }

            return image;
        }

        public static string ToImageDataURL(this byte[] image)
        {
            if (image.Length > 0)
            {
                string imageBase64 = Convert.ToBase64String(image);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64);

                return imageDataURL;
            }

            return string.Empty;
        }
    }
}
