using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Enums;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicEvents.Infrastructure.Repositories;

/// <summary>
/// Event repository. Uses the DbContext to access PostgreSQL.
/// </summary>
public class EventRepository : IEventRepository
{
    private readonly AcademicEventsDbContext _context;

    public EventRepository(AcademicEventsDbContext context)
    {
        _context = context;
    }

    public async Task<Event> CreateAsync(Event eventEntity)
    {
        _context.Events.Add(eventEntity);

        await _context.SaveChangesAsync();
        await _context.Entry(eventEntity).Reference(e => e.Organizer).LoadAsync();

        return eventEntity;
    }

    public async Task<Event?> GetByIdAsync(int id)
    {
        // Includes the organizer to avoid an additional query when building the response.
        return await _context.Events
            .Include(e => e.Organizer)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Event>> GetAllAsync()
    {
        return await _context.Events
            .Include(e => e.Organizer)
            .OrderByDescending(e => e.StartAt)
            .ToListAsync();
    }

    public async Task<List<Event>> GetByStatusAsync(EventStatus eventStatus)
    {
        // Filters by status and includes the organizer to avoid an additional query.
        return await _context.Events
            .Include(e => e.Organizer)
            .Where(e => e.EventStatus == eventStatus)
            .OrderByDescending(e => e.StartAt)
            .ToListAsync();
    }

    public async Task<List<Event>> GetByOrganizerAsync(int organizerId)
    {
        return await _context.Events
            .Include(e => e.Organizer)
            .Where(e => e.OrganizerId == organizerId)
            .OrderByDescending(e => e.StartAt)
            .ToListAsync();
    }

    public async Task<List<Event>> GetFilteredAsync(EventStatus? status, int? organizerId)
    {
        IQueryable<Event> query = _context.Events.Include(e => e.Organizer);

        if (status.HasValue)
            query = query.Where(e => e.EventStatus == status.Value);

        if (organizerId.HasValue)
            query = query.Where(e => e.OrganizerId == organizerId.Value);

        return await query
            .OrderByDescending(e => e.StartAt)
            .ToListAsync();
    }

    public async Task<Event?> UpdateAsync(Event eventEntity)
    {
        _context.Events.Update(eventEntity);

        await _context.SaveChangesAsync();

        return eventEntity;
    }

    public async Task DeleteAsync(int id)
    {
        Event? eventEntity = await _context.Events.FindAsync(id);

        if (eventEntity is null)
            return;

        _context.Events.Remove(eventEntity);

        await _context.SaveChangesAsync();
    }
}