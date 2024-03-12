using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class FileDetails 
    {
        
        public int ID { get; set; }
        public string? FileName { get; set; }
        public byte[]? FileData { get; set; }
        public FileType FileType { get; set; }
    }
} 