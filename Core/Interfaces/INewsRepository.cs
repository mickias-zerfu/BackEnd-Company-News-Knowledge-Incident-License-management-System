using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface INewsRepository
    {
        Task<News> GetNewsByIdAsync(int id);
        Task<IReadOnlyList<News>> GetNewsAsync();
        Task<News> CreateNewsAsync(News news);
        Task<News> UpdateNewsAsync(News news);
        Task DeleteNewsAsync(int id);



        Task<Comment> GetCommentByIdAsync(int id);
        Task<IReadOnlyList<Comment>> GetCommentsForNewsAsync(int newsId);
        Task<Comment> CreateCommentAsync(Comment comment);
        Task<Comment> UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int id);
    }
}