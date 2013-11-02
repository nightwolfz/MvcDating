using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace MvcDating.Helpers
{
    /**
     * Upload and resize pictures
     */
    public class Upload
    {
        public static List<string> UploadAndRename(HttpPostedFileBase file)
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
            var fileName = Path.Combine(uploadFolder, Guid.NewGuid().ToString());

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
    }
}