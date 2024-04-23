using Core.Interfaces.auth;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Auth
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        public async Task<bool> IsValidUser(string username, string password)
        {
            //TODO: Add Active Directory logic
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, "Zemenbank.local"))
            {

                //UserPrincipal user = UserPrincipal.FindByIdentity(context, username);
                //if (user == null)
                //{
                //    return false;
                //}
                //if (!user.UserPrincipalName.Equals(username))
                //{
                //    return false;
                //}

                //if (!user.IsAccountLockedOut())
                //{
                //    return false;
                //}
                return await Task.FromResult(context.ValidateCredentials(username, password));
            }
        }
    }
}