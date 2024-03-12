using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class KnowledgeBase : BaseEntity
    {
        
    public string? Problem { get; set; }
    public string? ProblemDescription { get; set; }
    public string? Solution { get; set; }
    public string? Created_at { get; set; }
    public string? Updated_at { get; set; }
        
    }
}