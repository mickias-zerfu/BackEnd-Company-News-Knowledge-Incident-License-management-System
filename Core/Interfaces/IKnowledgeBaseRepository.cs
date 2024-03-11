

using Core.Entities;

namespace Core.Interfaces
{
    public interface IKnowledgeBaseRepository
    {
        Task<KnowledgeBase> GetKnowledgeBaseByIdAsync(int id);
        Task<IReadOnlyList<KnowledgeBase>> GetKnowledgeBasesAsync();
        Task<KnowledgeBase> CreateKnowledgeBaseAsync(KnowledgeBase knowledgeBase);
        Task<KnowledgeBase> UpdateKnowledgeBaseAsync(KnowledgeBase knowledgeBase);
        Task DeleteKnowledgeBaseAsync(int id);
    }
}