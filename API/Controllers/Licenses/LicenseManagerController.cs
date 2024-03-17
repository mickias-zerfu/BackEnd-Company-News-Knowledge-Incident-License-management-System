using Core.Entities;
using Core.Entities.licenseEntity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc; 

namespace API.Controllers.Licenses
{
    [ApiController]
    [Route("api/licensemanagers")]
    public class LicenseManagerController : ControllerBase
    {
        private readonly ILicenseManagerRepository _licenseManagerRepository;

        public LicenseManagerController(ILicenseManagerRepository licenseManagerRepository)
        {
            _licenseManagerRepository = licenseManagerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetLicenseManagers()
        {
            var licenseManagers = await _licenseManagerRepository.GetAllLicenseManagersAsync();
            return Ok(licenseManagers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLicenseManagerById(int id)
        {
            var licenseManager = await _licenseManagerRepository.GetLicenseManagerByIdAsync(id);
            if (licenseManager == null)
            {
                return NotFound();
            }
            return Ok(licenseManager);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLicenseManager([FromBody] LicenseManager licenseManager)
        {
            var createdLicenseManager = await _licenseManagerRepository.CreateLicenseManagerAsync(licenseManager);
            return CreatedAtAction(nameof(GetLicenseManagerById), new { id = createdLicenseManager.Id }, createdLicenseManager);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicenseManager(int id, [FromBody] LicenseManager licenseManager)
        {
            if (id != licenseManager.Id)
            {
                return BadRequest("License manager ID mismatch");
            }

            var existingLicenseManager = await _licenseManagerRepository.GetLicenseManagerByIdAsync(id);
            if (existingLicenseManager == null)
            {
                return NotFound();
            }

            await _licenseManagerRepository.UpdateLicenseManagerAsync(licenseManager);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicenseManager(int id)
        {
            var existingLicenseManager = await _licenseManagerRepository.GetLicenseManagerByIdAsync(id);
            if (existingLicenseManager == null)
            {
                return NotFound();
            }

            await _licenseManagerRepository.DeleteLicenseManagerAsync(id);
            return NoContent();
        }
    }
}
