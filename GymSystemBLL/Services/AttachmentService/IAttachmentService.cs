using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.AttachmentService
{
    public interface IAttachmentService
    {
        // Function to Upload Photo and Return Photo Name
        string? Upload(string folderName, IFormFile file);

        // Function to Delete Photo
        bool Delete(string folderName, string fileName);
    }
}
