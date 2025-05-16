using IdeaManager.Core.Entities;
using IdeaManager.Core.Interfaces;
using IdeaManager.Data.Db;
using Microsoft.EntityFrameworkCore;

namespace IdeaManager.Data.Repositories
{
    public interface IIdeaRepository : IRepository<Idea>
    {

    }
}

public class IdeaRepository : IRepository<Idea>
{
    private readonly IdeaDbContext _context;

    public IdeaRepository(IdeaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Idea>> GetAllAsync()
    {
        return await _context.Ideas.ToListAsync();
    }

    public async Task<Idea?> GetByIdAsync(int id)
    {
        return await _context.Ideas.FindAsync(id);
    }

    public async Task AddAsync(Idea idea)
    {
        _context.Ideas.Add(idea);
        await _context.SaveChangesAsync();
    }
}
