using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicEvents.Infrastructure.Repositories;

/// <summary>
/// Repository de comentários. Usa o DbContext para acessar o PostgreSQL.
/// </summary>
public class CommentRepository : ICommentRepository
{
    private readonly AcademicEventsDbContext _context;

    public CommentRepository(AcademicEventsDbContext context)
    {
        _context = context;
    }

    public async Task<Comment> CreateAsync(Comment comentario)
    {
        _context.Comments.Add(comentario);
        await _context.SaveChangesAsync();
        return comentario;
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Comment>> GetByEventoAsync(int eventoId)
    {
        return await _context.Comments
            .Include(c => c.Usuario)
            .Where(c => c.EventoId == eventoId)
            .OrderByDescending(c => c.CriadoEm)
            .ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Comment? comentario = await _context.Comments.FindAsync(id);
        if (comentario is null) return;
        _context.Comments.Remove(comentario);
        await _context.SaveChangesAsync();
    }
}
