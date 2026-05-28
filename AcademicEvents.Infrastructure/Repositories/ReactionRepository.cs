using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicEvents.Infrastructure.Repositories;

/// <summary>
/// Repository de reações. Usa o DbContext para acessar o PostgreSQL.
/// </summary>
public class ReactionRepository : IReactionRepository
{
    private readonly AcademicEventsDbContext _context;

    public ReactionRepository(AcademicEventsDbContext context)
    {
        _context = context;
    }

    public async Task<Reaction> CreateAsync(Reaction reacao)
    {
        _context.Reactions.Add(reacao);
        await _context.SaveChangesAsync();
        return reacao;
    }

    public async Task<Reaction?> GetByIdAsync(int id)
    {
        return await _context.Reactions
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Reaction>> GetByEventoAsync(int eventoId)
    {
        return await _context.Reactions
            .Include(r => r.Usuario)
            .Where(r => r.EventoId == eventoId)
            .ToListAsync();
    }

    public async Task<Reaction?> GetByUsuarioEEventoAsync(int usuarioId, int eventoId)
    {
        return await _context.Reactions
            .FirstOrDefaultAsync(r => r.UsuarioId == usuarioId && r.EventoId == eventoId);
    }

    public async Task DeleteAsync(int id)
    {
        Reaction? reacao = await _context.Reactions.FindAsync(id);
        if (reacao is null) return;
        _context.Reactions.Remove(reacao);
        await _context.SaveChangesAsync();
    }
}
