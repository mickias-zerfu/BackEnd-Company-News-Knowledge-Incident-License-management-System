 
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
                .Include(sp => sp.Licenses) // Include related licenses
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

        public async Task<SoftwareProduct> UpdateSoftwareProductAsync(SoftwareProduct softwareProduct)
        {
            _context.Entry(softwareProduct).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return softwareProduct;
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
