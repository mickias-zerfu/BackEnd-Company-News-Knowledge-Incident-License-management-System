using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly INewsRepository _repo;

        public CommentController(INewsRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetCommentById(int id)
        {
            var comment = await _repo.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        // [HttpGet("news/{newsId}")]
        // public async Task<ActionResult<IReadOnlyList<Comment>>> GetCommentsForNews(int newsId)
        // {
        //     var comments = await _repo.GetCommentsForNewsAsync(newsId);
        //     return Ok(comments);
        // }

        [HttpPost]
        public async Task<ActionResult<Comment>> CreateComment(Comment comment)
        {
            var createdComment = await _repo.CreateCommentAsync(comment);
            return CreatedAtAction(nameof(GetCommentById), new { id = createdComment.Id }, createdComment);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Comment>> UpdateComment(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            var existingComment = await _repo.GetCommentByIdAsync(id);
            if (existingComment == null)
            {
                return NotFound();
            }

            var updatedComment = await _repo.UpdateCommentAsync(comment);
            return Ok(updatedComment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var existingComment = await _repo.GetCommentByIdAsync(id);
            if (existingComment == null)
            {
                return NotFound();
            }

            await _repo.DeleteCommentAsync(id);
            return NoContent();
        }
    }
}