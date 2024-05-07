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
                UserPrincipal user = UserPrincipal.FindByIdentity(context, username);
                if (user == null)
                {
                    return new { status = 0, message = "User Not Found", response = new { } };
                }
                if (user.IsAccountLockedOut())
                {
                    return new { status = 0, message = "User Account Locked Out, Please inform your nearest admin!!", response = new { } };
                }
                if (!context.ValidateCredentials(username, password))
                {
                    return new { status = 0, message = " Invalid Credentials", response = new { } };
                }
                if (context.ValidateCredentials(username, password))
                {
                    return new
                    {
                        status = 1,
                        message = "Successfull Login",
                        response = new
                        {
                            user_id = user?.EmployeeId,
                            user_data = new
                            {
                                name = username,
                                email = user?.EmailAddress,
                                role_id = 0
                            }
                        },
                        token = "generate token wait "

                    };
                }
                return new { status = 0, message = " Invalid Credentials", response = new { } };
            }
        }
    }
}
                //if (context.ValidateCredentials(username, password))
                //{
                //    // Here, you can retrieve additional user data and return it
                //    // For example:
                //    var userPrincipal = UserPrincipal.FindByIdentity(context, username);
                //    // var userData = new
                //    // {
                //    //     Username = username,
                //    //     Email = userPrincipal?.EmailAddress,
                //    //     // Add other user details as needed
                //    // };
                //    return userPrincipal;
                //}
                //else
                //{
                //    // Authentication failed, return null or an error message
                //    return null;
                //}