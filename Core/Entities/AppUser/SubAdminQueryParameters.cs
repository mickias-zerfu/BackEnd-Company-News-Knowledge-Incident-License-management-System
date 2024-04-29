using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.AppUser
{
    public class SubAdminQueryParameters
    {
        public int PageNo { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string SearchItem { get; set; } 
    }

   
}