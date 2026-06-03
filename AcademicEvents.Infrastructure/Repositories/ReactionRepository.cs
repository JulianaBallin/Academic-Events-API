using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicEvents.Infrastructure.Repositories;

/// <summary>
/// Reaction repository. Uses the DbContext to access PostgreSQL.
/// </summary>
public class ReactionRepository : IReactionRepository
{
    private readonly AcademicEventsDbContext _context;

    public ReactionRepository(AcademicEventsDbContext context)
    {
        _context = context;
    }

    public async Task<Reaction> CreateAsync(Reaction reaction)
    {
        _context.Reactions.Add(reaction);
        await _context.SaveChangesAsync();
        await _context.Entry(reaction).Reference(r => r.User).LoadAsync();
        return reaction;
    }

    public async Task<Reaction?> GetByIdAsync(int id)
    {
        return await _context.Reactions
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Reaction>> GetByEventAsync(int eventId)
    {
        return await _context.Reactions
            .Include(r => r.User)
            .Where(r => r.EventId == eventId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Reaction?> GetByUserEventAsync(int userId, int eventId)
    {
        return await _context.Reactions
            .FirstOrDefaultAsync(r => r.UserId == userId && r.EventId == eventId);
    }

    public async Task DeleteAsync(int id)
    {
        Reaction? reaction = await _context.Reactions.FindAsync(id);
        if (reaction is null) return;
        _context.Reactions.Remove(reaction);
        await _context.SaveChangesAsync();
    }
}
