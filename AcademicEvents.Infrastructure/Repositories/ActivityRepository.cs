using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicEvents.Infrastructure.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly AcademicEventsDbContext _context;

    public ActivityRepository(AcademicEventsDbContext context)
    {
        _context = context;
    }

    public async Task<Activity> CreateAsync(Activity activity)
    {
        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();
        return activity;
    }

    public async Task<Activity?> GetByIdAsync(int id)
    {
        return await _context.Activities
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Activity>> GetByEventIdAsync(int eventId)
    {
        return await _context.Activities
            .Where(a => a.EventId == eventId)
            .OrderBy(a => a.StartAt)
            .ToListAsync();
    }

    public async Task UpdateAsync(Activity activity)
    {
        _context.Activities.Update(activity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Activity activity)
    {
        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync();
    }
}
