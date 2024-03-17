
using Core.Entities.licenseEntity;

namespace Core.Interfaces
{
    public interface ILicenseRepository
    {
        Task<License> GetLicenseByIdAsync(int id);
        Task<IReadOnlyList<License>> GetAllLicensesAsync();
        Task<License> CreateLicenseAsync(License license);
        Task<License> UpdateLicenseAsync(License license);
        Task DeleteLicenseAsync(int id);
    }

    public interface ISoftwareProductRepository
    {
        Task<SoftwareProduct> GetSoftwareProductByIdAsync(int id);
        Task<IReadOnlyList<SoftwareProduct>> GetAllSoftwareProductsAsync();
        Task<SoftwareProduct> CreateSoftwareProductAsync(SoftwareProduct softwareProduct);
        Task<SoftwareProduct> UpdateSoftwareProductAsync(SoftwareProduct softwareProduct);
        Task DeleteSoftwareProductAsync(int id); 
    }

    public interface ILicenseManagerRepository
    {
        Task<LicenseManager> GetLicenseManagerByIdAsync(int id);
        Task<IReadOnlyList<LicenseManager>> GetAllLicenseManagersAsync();
        Task<LicenseManager> CreateLicenseManagerAsync(LicenseManager licenseManager);
        Task<LicenseManager> UpdateLicenseManagerAsync(LicenseManager licenseManager);
        Task DeleteLicenseManagerAsync(int id);
        // Add other methods as needed
    }
}
