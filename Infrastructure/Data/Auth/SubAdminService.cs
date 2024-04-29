using Core.Entities.AppUser; 
using Core.Interfaces.auth; 
using Infrastructure.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Services
{
    public class SubAdminService : ISubAdminService
    {
        private readonly AppIdentityDbContext _context; // Your DbContext _context;

        public SubAdminService(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<object> AdminLoginAsync(string email, string password)
        {
            var user = await  GetSingleSubAdminAsync(email);

            if (user == null)
            {
                return new { status = 0, message = "Email not registered", response = new { } };
            }

            if (user.Status != 1)
            {
                return new { status = 0, message = "Your account is deactivated by admin.", response = new { } };
            }

            // if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            // {
            //     return new { status = 0, message = "Invalid email or password", response = new { } };
            // }
            if (password != user.PasswordHash)
            {
                return new { status = 0, message = "Invalid email or password", response = new { } };
            }

            var token = GenerateToken(user.Id);

            return new
            {
                status = 1,
                message = "Login Successfully",
                response = new
                {
                    user_id = user.Id,
                    user_data = new
                    {
                        name = user.Name, 
                        email = user.Email,  
                        role_id = user.RoleId,
                        access = user.Access
                    }
                },
                token = token
            };
        }

        private string GenerateToken(int userId)
        {
            // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            // var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // var token = new JwtSecurityToken(
            //     issuer: _configuration["Jwt:Issuer"],
            //     audience: _configuration["Jwt:Issuer"],
            //     claims: new[] { new System.Security.Claims.Claim("user_id", userId) },
            //     expires: DateTime.Now.AddMinutes(30),
            //     signingCredentials: creds);

            // return new JwtSecurityTokenHandler().WriteToken(token);
            return "stringtoken";
        } 
    public async Task<bool> InsertSubAdminAsync(SubAdmin subAdmin)
    {
        // Your logic for inserting a subadmin user
        subAdmin.Created_at = System.DateTime.Now;
        subAdmin.Updated_at = System.DateTime.Now;
        _context.SubAdmins.Add(subAdmin);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<List<SubAdmin>> GetSubAdminsAsync(SubAdminQueryParameters queryParameters)
    {
        IQueryable<SubAdmin> query = _context.SubAdmins;
        if (!string.IsNullOrEmpty(queryParameters.SearchItem))
        {
            query = query.Where(s =>
                s.Name.Contains(queryParameters.SearchItem) ||
                s.Email.Contains(queryParameters.SearchItem));
        }
        query = query.Skip((queryParameters.PageNo - 1) * queryParameters.Size)
                     .Take(queryParameters.Size);
        return await query.ToListAsync();
    }

    public async Task<SubAdmin> GetSingleSubAdminAsync(string userEmail)
    {
        return await _context.SubAdmins
            .FirstOrDefaultAsync(s => s.Email == userEmail);
    }

    public async Task<bool> UpdateSubAdminAsync(SubAdmin subAdmin)
    {
        if (subAdmin == null || subAdmin.Id <= 0)
            return false;
        var existingSubAdmin = await _context.SubAdmins.FindAsync(subAdmin.Id);
        if (existingSubAdmin == null)
            return false;
        subAdmin.Updated_at = System.DateTime.Now;
        _context.Entry(subAdmin).State = EntityState.Modified;
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }
    public async Task<bool> DeleteSubAdminAsync(int userId)
    {
        var subAdmin = await _context.SubAdmins.FindAsync(userId);
        if (subAdmin == null)
            return false;

        _context.SubAdmins.Remove(subAdmin);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> InactivateSubAdminAsync(int subAdminId)
    {
        var subAdmin = await _context.SubAdmins.FindAsync(subAdminId);
        if (subAdmin == null)
            return false;

        subAdmin.Status = 0; // Assuming Status property exists in your SubAdmin entity
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ActivateSubAdminAsync(int subAdminId)
    {
        var subAdmin = await _context.SubAdmins.FindAsync(subAdminId);
        if (subAdmin == null)
            return false;

        subAdmin.Status = 1; // Assuming Status property exists in your SubAdmin entity
        await _context.SaveChangesAsync();
        return true;
    }

}
}
