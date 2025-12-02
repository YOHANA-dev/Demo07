using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[] AllowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        private readonly long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public string? Upload(string folderName, IFormFile file)
        {
            try
            {
                // 1) Validate File
                if (folderName is null || file is null || file.Length == 0) return null;
                if (file.Length > MaxFileSize) return null;

                // 2) Check Extension
                var ext = Path.GetExtension(file.FileName).ToLower();
                if (!AllowedExtensions.Contains(ext)) return null;

                // 3) Get Folder Path
                var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", folderName);
                //var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
                //var FolderPath = Path.Combine(projectRoot, "wwwroot", folderName);
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }

                // 4) GUID
                var FileName = Guid.NewGuid().ToString() + ext;

                // 5) Get File Path
                var FilePath = Path.Combine(FolderPath, FileName);

                // 6) Stream
                using var FileStream = new FileStream(FilePath, FileMode.Create);

                // 7) Copy to Stream
                file.CopyTo(FileStream);

                // 8) Return File Name
                return FileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Upload Photo : {ex}");
                return null;
            }
        }

        public bool Delete(string folderName, string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(folderName) || string.IsNullOrEmpty(fileName)) return false;

                var FullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", folderName, fileName);
                if (File.Exists(FullPath))
                {
                    File.Delete(FullPath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Delete Photo : {ex}");
                return false;
            }
        }
    }
}
