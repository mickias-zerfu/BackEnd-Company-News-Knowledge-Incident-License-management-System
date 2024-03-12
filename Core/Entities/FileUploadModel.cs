using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Entities
{
    public class FileUploadModel
    {
        public IFormFile? FileDetails { get; set; }
        public FileType FileType { get; set; } 
        
    }
}