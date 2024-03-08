
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
        // public async Task<News> GetNewsByIdAsync(int id)
        // {
        //     var news = await _context.News.Include(n => n.Comments).FirstOrDefaultAsync(n => n.Id == 1);
        //     return news;
        // }
        // public async Task<News> GetNewsByIdAsync(int id)
        // {
        //     return await _context.News.Include(p => p.Comments)
        //     .FirstOrDefaultAsync(x => x.Id == id);

        //     // return await _context.News.FindAsync(id);
        // }
        public async Task<News> GetNewsByIdAsync(int id)
        {
            return await _context.News
        .Include(news => news.Comments.Where(comment => comment.NewsPostId == id))
        .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IReadOnlyList<Comment>> GetCommentsForNewsAsync(int newsId)
        {
            return await _context.Comments
                .Where(c => c.NewsPostId == newsId)
                .ToListAsync();
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
            _context.Entry(news).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return news;
        }
        public async Task<IReadOnlyList<Comment>> GetCommentByIdAsync(int id)
        {
            return await _context.Comments.Where(c => c.NewsPostId == id)
        .ToListAsync();
        }
        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return comment;
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