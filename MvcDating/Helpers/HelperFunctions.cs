using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace MvcDating.Helpers
{
    public class HelperFunctions
    {
        public static readonly string ItemUploadFolderPath = "~/Uploads/";

        public string UploadAndRename(HttpPostedFileBase file)
        {
            string finalFileName = "x_" + DateTime.Now.ToFileTime() + Path.GetExtension(file.FileName);
            return UploadAndResize(file, finalFileName);
        }

        public string UploadAndResize(HttpPostedFileBase file, string fileName)
        {
            var path = Path.Combine(HttpContext.Current.Request.MapPath(ItemUploadFolderPath), fileName);
            string extension = Path.GetExtension(file.FileName);

            // Make sure the file is valid
            if (!ValidateExtension(extension)) throw new Exception("Invalid image file extension");

            try
            {
                file.SaveAs(path);

                Image imgOriginal = Image.FromFile(path);

                // Small image
                Image imgActualSmall = ScaleBySize(imgOriginal, 150);
                imgActualSmall.Save(path.Replace("x_", "s_"));
                imgActualSmall.Dispose();

                // Bigger image
                //Image imgActualBig = ScaleBySize(imgOriginal, 720);
                //imgActualBig.Save(path);
                //imgActualBig.Dispose();

                imgOriginal.Dispose();
                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not resize or save resized file: " + ex.Message);
            }
        }

        private static bool ValidateExtension(string extension)
        {
            extension = extension.ToLower();
            switch (extension)
            {
                case ".jpg":
                    return true;
                case ".png":
                    return true;
                case ".gif":
                    return true;
                case ".jpeg":
                    return true;
                default:
                    return false;
            }
        }


        public static Image ScaleBySize(Image imgPhoto, int size)
        {
            int logoSize = size;

            float sourceWidth = imgPhoto.Width;
            float sourceHeight = imgPhoto.Height;
            float destHeight = 0;
            float destWidth = 0;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            // Resize Image to have the height = logoSize/2 or width = logoSize.
            // Height is greater than width, set Height = logoSize and resize width accordingly
            if (sourceWidth > (2 * sourceHeight))
            {
                destWidth = logoSize;
                destHeight = (float)(sourceHeight * logoSize / sourceWidth);
            }
            else
            {
                int h = logoSize / 2;
                destHeight = h;
                destWidth = (float)(sourceWidth * h / sourceHeight);
            }
            // Width is greater than height, set Width = logoSize and resize height accordingly

            var bmPhoto = new Bitmap((int)destWidth, (int)destHeight, PixelFormat.Format32bppPArgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, (int)destWidth, (int)destHeight),
                new Rectangle(sourceX, sourceY, (int)sourceWidth, (int)sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();

            return bmPhoto;
        }

        public static string GetAge(DateTime birthday)
        {
            DateTime now = DateTime.Today;
            int age = now.Year - birthday.Year;
            if (now < birthday.AddYears(age)) age--;

            return age.ToString(CultureInfo.InvariantCulture);
        }
    }
}