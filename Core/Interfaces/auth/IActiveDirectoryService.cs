using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.auth
{
    public interface IActiveDirectoryService
    {
        Task<bool> IsValidUser(string username, string password);
    }
}
