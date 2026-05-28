using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Enums;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicEvents.Infrastructure.Repositories;

/// <summary>
/// Repository de eventos. Usa o DbContext para acessar o PostgreSQL.
/// </summary>
public class EventRepository : IEventRepository
{
    private readonly AcademicEventsDbContext _context;

    public EventRepository(AcademicEventsDbContext context)
    {
        _context = context;
    }

    public async Task<Event> CreateAsync(Event evento)
    {
        _context.Events.Add(evento);
        await _context.SaveChangesAsync();
        return evento;
    }

    public async Task<Event?> GetByIdAsync(int id)
    {
        // já inclui o organizador pra não precisar de outra query na hora de montar o response
        return await _context.Events
            .Include(e => e.Organizador)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Event>> GetAllAsync()
    {
        return await _context.Events
            .Include(e => e.Organizador)
            .OrderByDescending(e => e.DataInicio)
            .ToListAsync();
    }

    public async Task<List<Event>> GetByStatusAsync(StatusEvento status)
    {
        // filtra pelo status e já inclui o organizador pra não precisar de outra query
        return await _context.Events
            .Include(e => e.Organizador)
            .Where(e => e.Status == status)
            .OrderByDescending(e => e.DataInicio)
            .ToListAsync();
    }

    public async Task<List<Event>> GetByOrganizadorAsync(int organizadorId)
    {
        return await _context.Events
            .Include(e => e.Organizador)
            .Where(e => e.OrganizadorId == organizadorId)
            .OrderByDescending(e => e.DataInicio)
            .ToListAsync();
    }

    public async Task<Event?> UpdateAsync(Event evento)
    {
        _context.Events.Update(evento);
        await _context.SaveChangesAsync();
        return evento;
    }

    public async Task DeleteAsync(int id)
    {
        Event? evento = await _context.Events.FindAsync(id);
        if (evento is null) return;
        _context.Events.Remove(evento);
        await _context.SaveChangesAsync();
    }
}
