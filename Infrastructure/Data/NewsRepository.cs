
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
            var news = await _context.News.ToListAsync();

            // var news = await _context.News.Include(n => n.Comments).ToListAsync();
            return news;
        }
        // public async Task<News> GetNewsByIdAsync(int id)
        // {
        //     var news = await _context.News.Include(n => n.Comments).FirstOrDefaultAsync(n => n.Id == 1);
        //     return news;
        // }
        public async Task<News> GetNewsByIdAsync(int id)
        {
            return await _context.News.Include(p => p.Comments)
            .FirstOrDefaultAsync(x => x.Id == id);

            // return await _context.News.FindAsync(id);
        }
        public async Task<IReadOnlyList<Comment>> GetCommentsForNewsAsync(int newsId)
        {
            return await _context.Comments
                .Where(c => c.NewsPostId == newsId)
                .ToListAsync();
        }
        public Task<News> CreateNewsAsync(News news)
        {
            throw new NotImplementedException();
        }

        public Task DeleteNewsAsync(int id)
        {
            throw new NotImplementedException();
        }


        public Task<News> UpdateNewsAsync(News news)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> GetCommentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }


        public Task<Comment> CreateCommentAsync(Comment comment)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> UpdateCommentAsync(Comment comment)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCommentAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}