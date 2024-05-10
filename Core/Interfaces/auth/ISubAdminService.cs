using Core.Entities.AppUser;

namespace Core.Interfaces.auth
{
    public interface ISubAdminService
    {


        string CreateToken(SubAdmin user);
        // Task<bool> InsertSubAdminAsync(SubAdmin subAdmin);
        // Task<List<SubAdmin>> GetSubAdminsAsync(SubAdminQueryParameters queryParameters);
        // Task<SubAdmin> GetSingleSubAdminAsync(string userId);
        // Task<bool> UpdateSubAdminAsync(SubAdmin subAdmin);
        // Task<bool> DeleteSubAdminAsync(int userId);        
        // Task<bool> InactivateSubAdminAsync(int subAdminId);
        // Task<bool> ActivateSubAdminAsync(int subAdminId);
        // Task<object> AdminLoginAsync(string email, string password); 
    }
}