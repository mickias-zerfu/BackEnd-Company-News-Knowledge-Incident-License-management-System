using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{

    [Authorize(Roles = "Admin,SubAdmin")]
    [ApiController]
    [Route("api/[controller]")]
    public class KnowledgeBaseController : ControllerBase
    {
        private readonly IKnowledgeBaseRepository _repository;

        public KnowledgeBaseController(IKnowledgeBaseRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KnowledgeBase>> GetKnowledgeBaseById(int id)
        {
            var knowledgeBase = await _repository.GetKnowledgeBaseByIdAsync(id);
            if (knowledgeBase == null)
            {
                return NotFound();
            }

            return knowledgeBase;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<KnowledgeBase>>> GetKnowledgeBases()
        {
            var knowledgeBases = await _repository.GetKnowledgeBasesAsync();
            return Ok(knowledgeBases);
        }

        [HttpPost]
        public async Task<ActionResult<KnowledgeBase>> CreateKnowledgeBase(KnowledgeBase knowledgeBase)
        {
            var createdKnowledgeBase = await _repository.CreateKnowledgeBaseAsync(knowledgeBase);
            return CreatedAtAction(nameof(GetKnowledgeBaseById), new { id = createdKnowledgeBase.Id }, createdKnowledgeBase);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<KnowledgeBase>> UpdateKnowledgeBase(int id, KnowledgeBase knowledgeBase)
        {
            if (id != knowledgeBase.Id)
            {
                return BadRequest();
            }

            var updatedKnowledgeBase = await _repository.UpdateKnowledgeBaseAsync(knowledgeBase);
            return Ok(updatedKnowledgeBase);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKnowledgeBase(int id)
        {
            await _repository.DeleteKnowledgeBaseAsync(id);
            return NoContent();
        }
    }
}