using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly INewsRepository _newsRepository;

        public CommentController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        [HttpGet("{newsId}/comments")]
        public async Task<ActionResult<IReadOnlyList<Comment>>> GetCommentsByNewsIdAsync(int newsId)
        {
            var news = await _newsRepository.GetNewsByIdAsync(newsId);
            if (news == null)
            {
                return NotFound();
            }

            return Ok(news.Comments);
        }

        [HttpPost("{newsId}/comments")]
        public async Task<ActionResult<Comment>> CreateCommentAsync(int newsId, Comment comment)
        {
            var news = await _newsRepository.GetNewsByIdAsync(newsId);
            if (news == null)
            {
                return NotFound();
            }

            comment.NewsId = newsId;
            var createdComment = await _newsRepository.CreateCommentAsync(newsId, comment);
            return createdComment;
            
        }

        [HttpPut("comments/{id}")]
        public async Task<IActionResult> UpdateCommentAsync(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest("Comment ID mismatch");
            }

            var existingComment = await _newsRepository.GetCommentByIdAsync(id);
            if (existingComment == null)
            {
                return NotFound();
            }

            await _newsRepository.UpdateCommentAsync(comment);
            return NoContent();
        }

        [HttpDelete("comments/{id}")]
        public async Task<IActionResult> DeleteCommentAsync(int id)
        {
            var existingComment = await _newsRepository.GetCommentByIdAsync(id);
            if (existingComment == null)
            {
                return NotFound();
            }

            await _newsRepository.DeleteCommentAsync(id);
            return NoContent();
        }
    }
}
