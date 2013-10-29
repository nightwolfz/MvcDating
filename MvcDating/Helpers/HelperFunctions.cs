using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using ImageResizer;

namespace MvcDating.Helpers
{
    public class HelperFunctions
    {

        /**
         * Facebook like "time ago"
         */
        public static string GetTimeAgo(DateTime timestamp)
        {
            TimeSpan diff = DateTime.Now.Subtract(timestamp);

            if (diff.TotalDays > 1 && diff.TotalDays < 4)   return TimeAgoFormat(diff.TotalDays, "day");
            if (diff.TotalHours > 1)                        return TimeAgoFormat(diff.TotalHours, "hour");
            if (diff.TotalMinutes > 1)                      return TimeAgoFormat(diff.TotalMinutes, "minute");
            if (diff.TotalMinutes < 1)                      return "A few seconds ago";
            
            return timestamp.ToString("MMM dd, yyyy");
        }
        private static string TimeAgoFormat(double time, string timeUnit)
        {
            return String.Format("{0} {1}{2} ago", (int)time, timeUnit, (time >= 2 ? "s" : ""));
        }


        /**
         * Upload and resize pictures
         */
        public List<string> UploadAndRename(HttpPostedFileBase file)
        {
            const string uploadFolder = "~/Uploads/";

            if (file == null || file.ContentLength <= 0) throw new Exception("Image error: File is null or has no content.");

            var generatedFiles = new List<string>();
            var versions = new Dictionary<string, string>
            {
                {"_s", "width=160&height=160&crop=auto&format=jpg"},
                {"_x", "maxwidth=1280&maxheight=1280&format=jpg"}
            };

            // Generate a filename (GUIDs are best).
            var fileName = Path.Combine(uploadFolder, System.Guid.NewGuid().ToString());

            // Generate each version
            foreach (string suffix in versions.Keys)
            {
                // Let the image builder add the correct extension based on the output file type
                var imageJob = new ImageResizer.ImageJob(file, fileName + suffix + ".<ext>", 
                    new ImageResizer.Instructions(versions[suffix]));
                imageJob.Build();
                generatedFiles.Add(fileName.Replace(uploadFolder, "") + suffix + ".jpg");
            }

            return generatedFiles;
        }

        /**
         * Get age based on birthday
         */
        public static string GetAge(DateTime birthday)
        {
            var now = DateTime.Today;
            int age = now.Year - birthday.Year;
            if (now < birthday.AddYears(age)) age--;

            return age.ToString(CultureInfo.InvariantCulture);
        }
    }
}