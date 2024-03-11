using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentController : ControllerBase
    {
        private readonly IIncidentRepository _repository;

        public IncidentController(IIncidentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Incident>> GetIncidentById(int id)
        {
            var incident = await _repository.GetIncidentByIdAsync(id);
            if (incident == null)
            {
                return NotFound();
            }

            return incident;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Incident>>> GetIncidents()
        {
            var incidents = await _repository.GetIncidentsAsync();
            return Ok(incidents);
        }

        [HttpPost]
        public async Task<ActionResult<Incident>> CreateIncident(Incident incident)
        {
            var createdIncident = await _repository.CreateIncidentAsync(incident);
            return CreatedAtAction(nameof(GetIncidentById), new { id = createdIncident.Id }, createdIncident);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Incident>> UpdateIncident(int id, Incident incident)
        {
            if (id != incident.Id)
            {
                return BadRequest();
            }

            var updatedIncident = await _repository.UpdateIncidentAsync(incident);
            return Ok(updatedIncident);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncident(int id)
        {
            await _repository.DeleteIncidentAsync(id);
            return NoContent();
        }
    }
}