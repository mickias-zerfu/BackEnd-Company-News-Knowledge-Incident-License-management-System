using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 

namespace API.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "Admin,SubAdmin,User")]
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
        [Authorize(Roles = "Admin,SubAdmin,User")]
        public async Task<ActionResult<IReadOnlyList<SharedResource>>> GetSharedResources()
        {
            var sharedResources = await _repository.GetSharedResourcesAsync();
            
            return Ok(sharedResources);
        }
         
        [HttpPost]
        [Authorize(Roles = "Admin,SubAdmin")]
        public async Task<ActionResult<SharedResource>> CreateSharedResource([FromForm] SharedResourceUploadModel fileDetails)
        {
            if (fileDetails == null)
            {
                return BadRequest();
            }

            try
            { 
                await _repository.CreateSharedResourceAsync(fileDetails);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SubAdmin")]
        public async Task<ActionResult<SharedResource>> UpdateSharedResource(int id, [FromForm] SharedResourceUploadModel sharedResource)
        {
            if (sharedResource == null)
            {
                return BadRequest();
            }

            try
            {
                var updatedSharedResource = await _repository.UpdateSharedResourceAsync(id, sharedResource);
                return Ok(updatedSharedResource);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSharedResource(int id)
        {
            await _repository.DeleteSharedResourceAsync(id);
            return NoContent();
        }

        [HttpGet("DownloadFile/{id}")]
        [Authorize(Roles = "Admin,SubAdmin,User")]
        public async Task<ActionResult> DownloadFile(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            try
            {
                string fileUrl = await _repository.DownloadFileById(id);
                return Ok(new { FileUrl = fileUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while downloading the file: {ex.Message}");
            }
        }


    }
}