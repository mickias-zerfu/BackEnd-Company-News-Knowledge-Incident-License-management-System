using Core.Entities.AppUser;
using Core.Interfaces.auth;
using Infrastructure.Data.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text; 
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class SubAdminService : ISubAdminService
    {
        private readonly AppIdentityDbContext _context; // Your DbContext _context;
        
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public SubAdminService(AppIdentityDbContext context, IConfiguration config)
        {
            _context = context;            
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
        }
        public string CreateToken(SubAdmin user)
        {
          
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.DisplayName) 
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
                Issuer = _config["Token:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
