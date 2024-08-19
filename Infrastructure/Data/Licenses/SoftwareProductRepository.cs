using Core.Entities.licenseEntity;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore; 

namespace Infrastructure.Data.Licenses
{
    public class SoftwareProductRepository : ISoftwareProductRepository
    {
        private readonly StoreContext _context;

        public SoftwareProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<SoftwareProduct> GetSoftwareProductByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID", nameof(id));
            }

            return await _context.SoftwareProducts
                .AsNoTracking()
                .FirstOrDefaultAsync(sp => sp.Id == id);
        }

        public async Task<IReadOnlyList<SoftwareProduct>> GetAllSoftwareProductsAsync()
        {
            return await _context.SoftwareProducts
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SoftwareProduct> CreateSoftwareProductAsync(SoftwareProduct softwareProduct)
        {
            if (softwareProduct == null)
            {
                throw new ArgumentNullException(nameof(softwareProduct), "Software product cannot be null.");
            }

            await _context.SoftwareProducts.AddAsync(softwareProduct);
            await _context.SaveChangesAsync();
            return softwareProduct;
        }

        public async Task<SoftwareProduct> UpdateSoftwareProductAsync(int id, SoftwareProduct softwareProduct)
        {
            if (softwareProduct == null)
            {
                throw new ArgumentNullException(nameof(softwareProduct), "Software product cannot be null.");
            }

            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID", nameof(id));
            }

            var existingSoftwareProduct = await _context.SoftwareProducts.FindAsync(id);
            if (existingSoftwareProduct == null)
            {
                return null; // Not found
            }

            // Update the existing software product properties
            existingSoftwareProduct.Name = softwareProduct.Name ?? existingSoftwareProduct.Name;
            existingSoftwareProduct.Version = softwareProduct.Version ?? existingSoftwareProduct.Version;
            existingSoftwareProduct.Description = softwareProduct.Description ?? existingSoftwareProduct.Description;
            existingSoftwareProduct.Vendor = softwareProduct.Vendor ?? existingSoftwareProduct.Vendor;
            existingSoftwareProduct.ReleaseDate = softwareProduct.ReleaseDate != DateTime.MinValue
                ? softwareProduct.ReleaseDate
                : existingSoftwareProduct.ReleaseDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency issues
                throw new Exception("Concurrency error occurred while updating the software product.", ex);
            }

            return existingSoftwareProduct;
        }

        public async Task DeleteSoftwareProductAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID", nameof(id));
            }

            var softwareProductToDelete = await _context.SoftwareProducts.FindAsync(id);
            if (softwareProductToDelete != null)
            {
                _context.SoftwareProducts.Remove(softwareProductToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
