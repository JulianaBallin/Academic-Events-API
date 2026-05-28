using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicEvents.Infrastructure.Repositories;

/// <summary>
/// Repository de inscrições. Usa o DbContext para acessar o PostgreSQL.
/// </summary>
public class RegistrationRepository : IRegistrationRepository
{
    private readonly AcademicEventsDbContext _context;

    public RegistrationRepository(AcademicEventsDbContext context)
    {
        _context = context;
    }

    public async Task<Registration> CreateAsync(Registration inscricao)
    {
        _context.Registrations.Add(inscricao);
        await _context.SaveChangesAsync();
        return inscricao;
    }

    public async Task<Registration?> GetByIdAsync(int id)
    {
        return await _context.Registrations
            .Include(r => r.Usuario)
            .Include(r => r.Evento)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Registration>> GetByUsuarioAsync(int usuarioId)
    {
        return await _context.Registrations
            .Include(r => r.Evento)
            .Where(r => r.UsuarioId == usuarioId)
            .OrderByDescending(r => r.CriadoEm)
            .ToListAsync();
    }

    public async Task<List<Registration>> GetByEventoAsync(int eventoId)
    {
        return await _context.Registrations
            .Include(r => r.Usuario)
            .Where(r => r.EventoId == eventoId)
            .ToListAsync();
    }

    public async Task<Registration?> GetByUsuarioEEventoAsync(int usuarioId, int eventoId)
    {
        return await _context.Registrations
            .FirstOrDefaultAsync(r => r.UsuarioId == usuarioId && r.EventoId == eventoId);
    }

    public async Task<Registration?> UpdateAsync(Registration inscricao)
    {
        _context.Registrations.Update(inscricao);
        await _context.SaveChangesAsync();
        return inscricao;
    }

    public async Task DeleteAsync(int id)
    {
        Registration? inscricao = await _context.Registrations.FindAsync(id);
        if (inscricao is null) return;
        _context.Registrations.Remove(inscricao);
        await _context.SaveChangesAsync();
    }
}
