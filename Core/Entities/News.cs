using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class News : BaseEntity
    {
    public News()
    {
        Comments = new List<Comment>();
    }

    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Image_url { get; set; }
    public string? Created_at { get; set; }
    public string? Updated_at { get; set; } 
    public ICollection<Comment>? Comments { get; set; }
    }
}