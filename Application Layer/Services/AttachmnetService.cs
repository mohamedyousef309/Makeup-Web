using Domain_Layer.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Services
{
    public class AttachmnetService: IAttachmentService
    {
        List<string> allowedExtensions = new List<string> { ".jpg", ".png", ".jpeg" };
        const int maxsize = 2097152; // 2MB in bytes
        public string? UploadImage(IFormFile file, string folderName)
        {
            //1
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension)) return null;
            //2
            if (file.Length > maxsize || file.Length == 0) return null;
            //3
            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);
            //4
            var FileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

            //5

            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            var filePath = Path.Combine(FolderPath, FileName);
            //6
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return "/" + Path.Combine("Files", folderName, FileName).Replace("\\", "/");
        }

       public bool DeleteImage(string imagePath)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.Replace("\\", "/"));

            if (!File.Exists(fullPath))
                return false;

            File.Delete(fullPath);
            return true;

        }

    }
}
