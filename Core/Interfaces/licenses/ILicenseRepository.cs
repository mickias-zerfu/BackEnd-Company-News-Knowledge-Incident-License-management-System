
using Core.Entities.licenseEntity;

namespace Core.Interfaces
{
    public interface ILicenseRepository
    {
        Task<LicenseDto> GetLicenseByIdAsync(int id);
        Task<IReadOnlyList<LicenseDto>> GetAllLicensesAsync();
        Task<License> CreateLicenseAsync(License license);
        Task<License> AssignManagersToLicenseAsync(int licenseId, int[] managerId);
        Task<License> UpdateLicenseAsync(License license);
        Task DeleteLicenseAsync(int id);
    }

    public interface ISoftwareProductRepository
    {
        Task<SoftwareProduct> GetSoftwareProductByIdAsync(int id);
        Task<IReadOnlyList<SoftwareProduct>> GetAllSoftwareProductsAsync();
        Task<SoftwareProduct> CreateSoftwareProductAsync(SoftwareProduct softwareProduct);
        Task<SoftwareProduct> UpdateSoftwareProductAsync(int id, SoftwareProduct softwareProduct);
        Task DeleteSoftwareProductAsync(int id);
    }

    public interface ILicenseManagerRepository
    {
        Task<LicenseManagerDto> GetLicenseManagerByIdAsync(int id);
        Task<IReadOnlyList<LicenseManagerDto>> GetAllLicenseManagersAsync();
        Task<LicenseManager> CreateLicenseManagerAsync(LicenseManager licenseManager);
        Task<LicenseManager> AssignLicensesAsyncToManager(int ManagerId, int[] LicenseIds);
        Task<LicenseManager> UpdateLicenseManagerAsync(LicenseManager licenseManager);
        Task DeleteLicenseManagerAsync(int id);
        // Add other methods as needed
    }
}
