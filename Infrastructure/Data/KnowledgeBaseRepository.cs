using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class KnowledgeBaseRepository : IKnowledgeBaseRepository
{
    private readonly StoreContext _Context;

    public KnowledgeBaseRepository(StoreContext Context)
    {
        _Context = Context;
    }

    public async Task<KnowledgeBase> GetKnowledgeBaseByIdAsync(int id)
    {
        return await _Context.KnowledgeBases.FindAsync(id);
    }

    public async Task<IReadOnlyList<KnowledgeBase>> GetKnowledgeBasesAsync()
    {
        return await _Context.KnowledgeBases.ToListAsync();
    }

    public async Task<KnowledgeBase> CreateKnowledgeBaseAsync(KnowledgeBase knowledgeBase)
    {
        _Context.KnowledgeBases.Add(knowledgeBase);
        await _Context.SaveChangesAsync();
        return knowledgeBase;
    }

    public async Task<KnowledgeBase> UpdateKnowledgeBaseAsync(KnowledgeBase knowledgeBase)
    {
        _Context.Entry(knowledgeBase).State = EntityState.Modified;
        await _Context.SaveChangesAsync();
        return knowledgeBase;
    }

    public async Task DeleteKnowledgeBaseAsync(int id)
    {
        var knowledgeBase = await _Context.KnowledgeBases.FindAsync(id);
        if (knowledgeBase != null)
        {
            _Context.KnowledgeBases.Remove(knowledgeBase);
            await _Context.SaveChangesAsync();
        }
    }
}