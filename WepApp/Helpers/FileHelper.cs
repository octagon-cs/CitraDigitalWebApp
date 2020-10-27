using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace WebApp.Helpers
{
    public class FileHelper
    {

        public static string PhotoPath => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos");
        public static string DocumentPath => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents");
        public static byte[] CreateThumb(byte[] byteArray)
        {
            try
            {
                Image imThumbnailImage;
                System.Drawing.Image OriginalImage;
                MemoryStream ms = new MemoryStream();

                // Stream / Write Image to Memory Stream from the Byte Array.
                ms.Write(byteArray, 0, byteArray.Length);

                OriginalImage = System.Drawing.Image.FromStream(ms);

                // Shrink the Original Image to a thumbnail size.
                imThumbnailImage = OriginalImage.GetThumbnailImage(100, 100,
                 new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallBack), IntPtr.Zero);

                // Save Thumbnail to Memory Stream for Conversion to Byte Array.
                MemoryStream myMS = new MemoryStream();
                imThumbnailImage.Save(myMS, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] test_imge = myMS.ToArray();
                return test_imge;
            }
            catch (System.Exception)
            {
                return byteArray;
            }
        }


        internal static async Task SaveFile(byte[] data, string extention, PathType pathType)
        {
            try
            {
                var fileName = Helpers.FileHelper.CreateFileName(extention, pathType);
                await System.IO.File.WriteAllBytesAsync(fileName, data, new System.Threading.CancellationToken());
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        internal static async Task<string> SaveTruckFile(FileData file, PathType pathType)
        {
            try
            {
                var fileName = Helpers.FileHelper.CreateFileName(file.FileExtention, pathType);

                await System.IO.File.WriteAllBytesAsync(fileName, file.Data, new System.Threading.CancellationToken());
                return fileName;
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        internal static string CreateFileName(string fileType, PathType type)
        {
            Guid guid = Guid.NewGuid();
            var path = type == PathType.Photo ? PhotoPath : DocumentPath;

            return path + "/" + guid.ToString() + "." + fileType;
        }


        private static bool ThumbnailCallBack()
        {
            return false;
        }

    }


    public enum PathType
    {
        Photo, Document
    }
}