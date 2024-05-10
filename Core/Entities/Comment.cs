 
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int NewsId { get; set; }
        [ForeignKey("NewsId")]
         [JsonIgnore] 
        public News? News { get; set; }

    }
}