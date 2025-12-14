using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interfaces.ServiceInterfaces
{
    public interface IAttachmentService
    {
        string? UploadImage(IFormFile file, string folderName);

        public bool DeleteImage(string imagePath);

    }
}
