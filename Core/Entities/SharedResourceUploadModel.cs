using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Entities
{
    public class SharedResourceUploadModel
    {
        
        public string? FileTitle { get; set; }
        public string? FileDescription { get; set; }
        public IFormFile? FileDetails { get; set; } 
        public FileType FileType { get; set; } 
        
    }
}