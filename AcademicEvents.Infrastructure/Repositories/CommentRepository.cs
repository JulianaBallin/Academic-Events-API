using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicEvents.Infrastructure.Repositories;

/// <summary>
/// Comment repository. Uses the DbContext to access PostgreSQL.
/// </summary>
public class CommentRepository : ICommentRepository
{
    private readonly AcademicEventsDbContext _context;

    public CommentRepository(AcademicEventsDbContext context)
    {
        _context = context;
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        await _context.Entry(comment).Reference(c => c.User).LoadAsync();
        return comment;
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Comment>> GetByEventAsync(int eventId)
    {
        return await _context.Comments
            .Include(c => c.User)
            .Where(c => c.EventId == eventId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Comment? comment = await _context.Comments.FindAsync(id);
        if (comment is null) return;
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }
}
