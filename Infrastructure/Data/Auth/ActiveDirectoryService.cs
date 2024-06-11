using Core.Interfaces.auth;
using System.DirectoryServices.AccountManagement;
using Core.Entities.AppUser;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;  
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Auth
{
    public class ActiveDirectoryService : IActiveDirectoryService
    { 
        private readonly IConfiguration _config;
        private readonly ILogger<ActiveDirectoryService> _logger;

        private readonly SymmetricSecurityKey _key;
        public ActiveDirectoryService(IConfiguration config,
             ILogger<ActiveDirectoryService> logger)
        {        
            _config = config;
            _logger = logger;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));}

        public async Task<DomainDto> IsValidUser(string username, string password)
        {
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, "Zemenbank.local"))
            {
                UserPrincipal user = UserPrincipal.FindByIdentity(context, username);
                if (user == null)
                {
                    return new DomainDto { Status = 0, Message = "User Not Found " };
                }
                if (user.IsAccountLockedOut())
                {
                    return new DomainDto { Status = 0, Message = "User Account Locked Out, Please inform your nearest admin!! " };
                }
                if (!context.ValidateCredentials(username, password))
                {
                    return new DomainDto { Status = 0, Message = " Invalid Credentials" };
                }
                if (context.ValidateCredentials(username, password))
                {
                    return new DomainDto
                    {
                        DisplayName = username.IndexOf('.') == -1 ? username : username.Substring(0, username.IndexOf('.')),
                        Token = CreateToken(user),
                        Email = user?.EmailAddress,
                        Status = 1,
                        RoleId = 0,
                        Message = "Login Successfully",
                    };

                }
                return new DomainDto { Status = 0, Message = " Invalid Credentials" };
            }
        }


        public string CreateToken(UserPrincipal user )
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user?.EmailAddress),
                new Claim(ClaimTypes.Name, user?.UserPrincipalName),
                new Claim(ClaimTypes.Role, "User")

        }; 
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(10),
                SigningCredentials = creds,
                Issuer = _config["Token:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        public async Task<DomainDto> GetCurrentUser(string username)
        {
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, "Zemenbank.local"))
            {
                UserPrincipal user = UserPrincipal.FindByIdentity(context, username);
                if (user == null)
                {
                    return new DomainDto { Status = 0, Message = "User Not Found" };
                }

                if (user.IsAccountLockedOut())
                {
                    return new DomainDto { Status = 0, Message = "User Account Locked Out, Please inform your nearest admin!!" };
                }

                return new DomainDto
                {
                    DisplayName = username.IndexOf('.') == -1 ? username : username.Substring(0, username.IndexOf('.')),
                    Token = CreateToken(user),
                    Email = user?.EmailAddress,
                    Status = 1,
                    RoleId = 0,
                    Message = "User Information Retrieved Successfully"
                };
            }
        }
    }
} 