using Core.Entities;
using Core.Entities.licenseEntity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.Licenses
{
    [ApiController]
    [Route("api/softwareproducts")]
    public class SoftwareProductController : ControllerBase
    {
        private readonly ISoftwareProductRepository _softwareProductRepository;

        public SoftwareProductController(ISoftwareProductRepository softwareProductRepository)
        {
            _softwareProductRepository = softwareProductRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetSoftwareProducts()
        {
            var softwareProducts = await _softwareProductRepository.GetAllSoftwareProductsAsync();
            return Ok(softwareProducts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSoftwareProductById(int id)
        {
            var softwareProduct = await _softwareProductRepository.GetSoftwareProductByIdAsync(id);
            if (softwareProduct == null)
            {
                return NotFound();
            }
            return Ok(softwareProduct);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSoftwareProduct([FromBody] SoftwareProduct softwareProduct)
        {
            var createdSoftwareProduct = await _softwareProductRepository.CreateSoftwareProductAsync(softwareProduct);
            return CreatedAtAction(nameof(GetSoftwareProductById), new { id = createdSoftwareProduct.Id }, createdSoftwareProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSoftwareProduct(int id, [FromBody] SoftwareProduct softwareProduct)
        {
            try
            {
                if (id != softwareProduct.Id)
                {
                    return BadRequest("Software product ID mismatch");
                }

                var existingSoftwareProduct = await _softwareProductRepository.GetSoftwareProductByIdAsync(id);
                if (existingSoftwareProduct == null)
                {
                    return NotFound();
                }

                var updatedSoftwareProduct = await _softwareProductRepository.UpdateSoftwareProductAsync(id, softwareProduct);
                if (updatedSoftwareProduct == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update software product.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating the software product: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSoftwareProduct(int id)
        {
            var existingSoftwareProduct = await _softwareProductRepository.GetSoftwareProductByIdAsync(id);
            if (existingSoftwareProduct == null)
            {
                return NotFound();
            }

            await _softwareProductRepository.DeleteSoftwareProductAsync(id);
            return NoContent();
        }
    }
}
