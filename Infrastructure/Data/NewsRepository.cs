
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class NewsRepository : INewsRepository
    {
        private readonly StoreContext _context;
        public NewsRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<News>> GetNewsAsync()
        {
            // var news = await _context.News.ToListAsync();

            var news = await _context.News.Include(n => n.Comments).ToListAsync();
            return news;
        }
        public async Task<News> GetNewsByIdAsync(int id)
        {
            return await _context.News
                .Include(n => n.Comments)
        .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<News> CreateNewsAsync(News news)
        {
            _context.News.Add(news);
            await _context.SaveChangesAsync();
            return news;
        }
        public async Task DeleteNewsAsync(int id)
        {
            var newsToDelete = await _context.News.FindAsync(id);
            if (newsToDelete != null)
            {
                _context.News.Remove(newsToDelete);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<News> UpdateNewsAsync(News news)
        {
            try
            {
                var existingEntity = await _context.News.FindAsync(news.Id);
                if (existingEntity == null)
                {
                    throw new Exception("News with the specified ID was not found.");
                }

                _context.Entry(existingEntity).CurrentValues.SetValues(news);
                await _context.SaveChangesAsync();

                return existingEntity;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("An error occurred while updating the news.", ex);
            }
        }

        public async Task<Comment> GetCommentByIdAsync(int commentsId)
        {

            return await _context.Comments
        .FirstOrDefaultAsync(x => x.Id == commentsId);
        }
        public async Task<IReadOnlyList<Comment>> GetCommentsByNewsIdAsync(int newsId)
        {
            return await _context.Comments
                .Where(c => c.NewsId == newsId)
                .ToListAsync();
        }
        public async Task<Comment> CreateCommentAsync(int newsId, Comment comment)
        { 
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {

            var existingComment = await _context.Comments.FindAsync(comment.Id);
            if (existingComment == null)
            {
                throw new ArgumentException($"License manager with ID {comment.Id} not found.");
            }
            _context.Entry(existingComment).State = EntityState.Detached;
            _context.Attach(comment);
            _context.Entry(comment).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return comment;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflicts, if necessary
                throw new Exception("Concurrency conflict occurred");
            }
        }
        public async Task DeleteCommentAsync(int id)
        {
            var commentToDelete = await _context.Comments.FindAsync(id);
            if (commentToDelete != null)
            {
                _context.Comments.Remove(commentToDelete);
                await _context.SaveChangesAsync();
            }
        }

    }
}