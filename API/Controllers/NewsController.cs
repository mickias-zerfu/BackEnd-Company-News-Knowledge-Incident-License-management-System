using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository _repo;
        public NewsController(INewsRepository repo)
        {
            _repo = repo;

        }

        [HttpGet]
        public async Task<ActionResult<List<News>>> GetNewses()
        {
            var news = await _repo.GetNewsAsync();
            return Ok(news);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNewsById(int id)
        {
            return await _repo.GetNewsByIdAsync(id);
        }



        [HttpPost]
        public async Task<ActionResult<News>> CreateNews(News news)
        {
            var createdNews = await _repo.CreateNewsAsync(news);
            return CreatedAtAction(nameof(GetNewsById), new { id = createdNews.Id }, createdNews);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<News>> UpdateNews(int id, News news)
        {
            if (id != news.Id)
            {
                return BadRequest("The ID in the request body does not match the ID in the URL.");
            }

            var existingNews = await _repo.GetNewsByIdAsync(id);
            if (existingNews == null)
            {
                return NotFound("News with the specified ID was not found.");
            }

            try
            {
                var updatedNews = await _repo.UpdateNewsAsync(news);
                return Ok(updatedNews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the news: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var existingNews = await _repo.GetNewsByIdAsync(id);
            if (existingNews == null)
            {
                return NotFound();
            }

            await _repo.DeleteNewsAsync(id);
            return NoContent();
        }

    }


}