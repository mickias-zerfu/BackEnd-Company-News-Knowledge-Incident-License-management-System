using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SharedResourceController : ControllerBase
    {
        private readonly ISharedResourceRepository _repository;

        public SharedResourceController(ISharedResourceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SharedResource>> GetSharedResourceById(int id)
        {
            var sharedResource = await _repository.GetSharedResourceByIdAsync(id);
            if (sharedResource == null)
            {
                return NotFound();
            }

            return sharedResource;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SharedResource>>> GetSharedResources()
        {
            var sharedResources = await _repository.GetSharedResourcesAsync();
            return Ok(sharedResources);
        }

        [HttpPost]
        public async Task<ActionResult<SharedResource>> CreateSharedResource([FromForm] SharedResourceUploadModel fileDetails)
        {
            if (fileDetails == null)
            {
                return BadRequest();
            }

            try
            {
                // var createdSharedResource = await _repository.CreateSharedResourceAsync(fileDetails.SharedResource, fileDetails.FileDetails);
                // return CreatedAtAction(nameof(GetSharedResourceById), new { id = createdSharedResource.Id }, createdSharedResource);

                await _repository.CreateSharedResourceAsync(fileDetails);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SharedResource>> UpdateSharedResource(int id, SharedResource sharedResource)
        {
            if (id != sharedResource.Id)
            {
                return BadRequest();
            }

            var updatedSharedResource = await _repository.UpdateSharedResourceAsync(sharedResource);
            return Ok(updatedSharedResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSharedResource(int id)
        {
            await _repository.DeleteSharedResourceAsync(id);
            return NoContent();
        }
        
        [HttpGet("DownloadFile/{id}")]
        public async Task<ActionResult> DownloadFile(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            try
            {
                await _repository.DownloadFileById(id);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}