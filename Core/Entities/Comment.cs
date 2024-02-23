using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Comment
    {

        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int NewsPostId { get; set; }
        // [ForeignKey("NewsPostId")]
        public News News { get; set; }

    }
}