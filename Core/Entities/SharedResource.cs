using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Entities
{
    public class SharedResource : BaseEntity
    {

        // public int ID { get; set; }
        public string? FileTitle { get; set; }
        public string? FileDescription { get; set; }
        public string? FileName { get; set; }
        //public byte[]? FileData { get; set; }
        public string? FilePath { get; set; } // Store file path
        public FileType FileType { get; set; }       
        public string? FileUrl { get; set; }
        public string? Created_at { get; set; }
        public string? Updated_at { get; set; }
 
    }
}
 
