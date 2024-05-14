using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.AppUser;

namespace Core.Interfaces.auth
{
    public interface IActiveDirectoryService
    {
        Task<DomainDto> GetCurrentUser(string name);
        Task<DomainDto> IsValidUser(string username, string password);
    }
}
