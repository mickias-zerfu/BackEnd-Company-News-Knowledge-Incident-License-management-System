
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
            return await _context.SoftwareProducts
                .FirstOrDefaultAsync(sp => sp.Id == id);
        }

        public async Task<IReadOnlyList<SoftwareProduct>> GetAllSoftwareProductsAsync()
        {
            return await _context.SoftwareProducts.ToListAsync();
        }

        public async Task<SoftwareProduct> CreateSoftwareProductAsync(SoftwareProduct softwareProduct)
        {
            _context.SoftwareProducts.Add(softwareProduct);
            await _context.SaveChangesAsync();
            return softwareProduct;
        }

        public async Task<SoftwareProduct> UpdateSoftwareProductAsync(int id, SoftwareProduct softwareProduct)
        {
            try
            {
                if (softwareProduct == null)
                {
                    throw new ArgumentNullException(nameof(softwareProduct), "Software product is null.");
                }

                var existingSoftwareProduct = await _context.SoftwareProducts.FindAsync(id);
                if (existingSoftwareProduct == null)
                {
                    // Software product with the specified ID not found
                    return null;
                }

                // Update the existing software product properties
                existingSoftwareProduct.Name = softwareProduct.Name ?? existingSoftwareProduct.Name;
                existingSoftwareProduct.Version = softwareProduct.Version ?? existingSoftwareProduct.Version;
                existingSoftwareProduct.Description = softwareProduct.Description ?? existingSoftwareProduct.Description;
                existingSoftwareProduct.Vendor = softwareProduct.Vendor ?? existingSoftwareProduct.Vendor;
                existingSoftwareProduct.ReleaseDate = softwareProduct.ReleaseDate != DateTime.MinValue ? softwareProduct.ReleaseDate : existingSoftwareProduct.ReleaseDate;

                // Save changes to the database
                await _context.SaveChangesAsync();

                return existingSoftwareProduct;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteSoftwareProductAsync(int id)
        {
            var softwareProductToDelete = await _context.SoftwareProducts.FindAsync(id);
            if (softwareProductToDelete != null)
            {
                _context.SoftwareProducts.Remove(softwareProductToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
