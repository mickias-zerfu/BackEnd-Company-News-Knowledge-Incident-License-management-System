using Core.Entities.licenseEntity;
using Core.Interfaces;
using Core.Interfaces.licenses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Licenses
{
    [ApiController]
    [Route("api/licenses")]
    public class LicenseController : ControllerBase
    {
        private readonly ILicenseRepository _licenseRepository;
        private readonly ILicenseExpirationService _licenseExpirationService;

        public LicenseController(ILicenseRepository licenseRepository, ILicenseExpirationService licenseExpirationService)
        {
            _licenseRepository = licenseRepository;
            _licenseExpirationService = licenseExpirationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLicenses()
        {
            var licenses = await _licenseRepository.GetAllLicensesAsync();
            return Ok(licenses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLicenseById(int id)
        {
            var license = await _licenseRepository.GetLicenseByIdAsync(id);
            if (license == null)
            {
                return NotFound();
            }
            return Ok(license);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLicense([FromBody] License license)
        {
            var createdLicense = await _licenseRepository.CreateLicenseAsync(license);
            return CreatedAtAction(nameof(GetLicenseById), new { id = createdLicense.Id }, createdLicense);
        }
        [HttpPost("assignmanager/{licenseId}")]
        public async Task<IActionResult> AssignManagersToLicense(int licenseId, [FromBody] int[] managerIds)
        {
            var updatedLicense = await _licenseRepository.AssignManagersToLicenseAsync(licenseId, managerIds);
            if (updatedLicense == null)
            {
                return NotFound();
            }

            return Ok(updatedLicense);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicense(int id, [FromBody] License license)
        {
            if (id != license.Id)
            {
                return BadRequest("License ID mismatch");
            }

            var existingLicense = await _licenseRepository.GetLicenseByIdAsync(id);
            if (existingLicense == null)
            {
                return NotFound();
            }

            await _licenseRepository.UpdateLicenseAsync(license);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicense(int id)
        {
            var existingLicense = await _licenseRepository.GetLicenseByIdAsync(id);
            if (existingLicense == null)
            {
                return NotFound();
            }

            await _licenseRepository.DeleteLicenseAsync(id);
            return NoContent();
        }

        [HttpPost("checkexpiration")]
        public async Task<IActionResult> CheckLicenseExpiration()
        {
            await _licenseExpirationService.CheckLicenseExpirationAsync();
            return Ok(new { message = "License expiration check completed successfully." });
        }
        [HttpPost("checkexpiration/{licenseId}")]
        public async Task<IActionResult> CheckLicenseExpirationPerId(int licenseId)
        {
            await _licenseExpirationService.CheckLicenseExpirationAsync();
            return Ok(new { message = "License expiration check completed successfully." });
        }
    }
}
