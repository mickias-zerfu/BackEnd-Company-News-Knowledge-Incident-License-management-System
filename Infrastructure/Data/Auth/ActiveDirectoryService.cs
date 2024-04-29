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
        public async Task<object> IsValidUser(string username, string password)
        {
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
                // return await Task.FromResult(context.ValidateCredentials(username, password));
                if (context.ValidateCredentials(username, password))
                {
                    // Here, you can retrieve additional user data and return it
                    // For example:
                    var userPrincipal = UserPrincipal.FindByIdentity(context, username);
                    // var userData = new
                    // {
                    //     Username = username,
                    //     Email = userPrincipal?.EmailAddress,
                    //     // Add other user details as needed
                    // };
                    return userPrincipal;
                }
                else
                {
                    // Authentication failed, return null or an error message
                    return null;
                }
            }
        }
    }
}