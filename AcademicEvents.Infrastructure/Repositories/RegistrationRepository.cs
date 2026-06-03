using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicEvents.Infrastructure.Repositories;

/// <summary>
/// Registration repository. Uses the DbContext to access PostgreSQL.
/// </summary>
public class RegistrationRepository : IRegistrationRepository
{
    private readonly AcademicEventsDbContext _context;

    public RegistrationRepository(AcademicEventsDbContext context)
    {
        _context = context;
    }

    public async Task<Registration> CreateAsync(Registration registration)
    {
        _context.Registrations.Add(registration);
        await _context.SaveChangesAsync();
        await _context.Entry(registration).Reference(r => r.User).LoadAsync();
        await _context.Entry(registration).Reference(r => r.Event).LoadAsync();
        return registration;
    }

    public async Task<Registration?> GetByIdAsync(int id)
    {
        return await _context.Registrations
            .Include(r => r.User)
            .Include(r => r.Event)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Registration>> GetByUserAsync(int userId)
    {
        return await _context.Registrations
            .Include(r => r.User)
            .Include(r => r.Event)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Registration>> GetByEventAsync(int eventId)
    {
        return await _context.Registrations
            .Include(r => r.User)
            .Where(r => r.EventId == eventId)
            .ToListAsync();
    }

    public async Task<Registration?> GetByUserEventAsync(int userId, int eventId)
    {
        return await _context.Registrations
            .FirstOrDefaultAsync(r => r.UserId == userId && r.EventId == eventId);
    }

    public async Task<Registration?> UpdateAsync(Registration registration)
    {
        _context.Registrations.Update(registration);
        await _context.SaveChangesAsync();
        return registration;
    }

    public async Task DeleteAsync(int id)
    {
        Registration? registration = await _context.Registrations.FindAsync(id);
        if (registration is null) return;
        _context.Registrations.Remove(registration);
        await _context.SaveChangesAsync();
    }
}
