using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class News
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? content { get; set; } 
        public string? image_url { get; set; }
        public string? created_at { get; set; }
        public string? updated_at { get; set; }
    }
}