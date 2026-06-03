using AcademicEvents.Application.DTOs.Activity;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;

namespace AcademicEvents.Application.Services;

/// <summary>
/// Service for managing academic event activities.
/// </summary>
public class ActivityService : IActivityService
{
    private readonly IActivityRepository _repository;
    private readonly IEventRepository _eventRepository;

    public ActivityService(IActivityRepository repository, IEventRepository eventRepository)
    {
        _repository = repository;
        _eventRepository = eventRepository;
    }

    public async Task<ActivityResponse> CreateAsync(CreateActivityRequest request, int userId)
    {
        string title = (request.Title ?? string.Empty).Trim();
        string description = (request.Description ?? string.Empty).Trim();
        string location = (request.Location ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(title))
            throw new InvalidOperationException("Title is required.");

        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidOperationException("Description is required.");

        if (string.IsNullOrWhiteSpace(location))
            throw new InvalidOperationException("Location is required.");

        if (request.StartAt is null)
            throw new InvalidOperationException("Start date is required.");

        if (request.EndedAt is null)
            throw new InvalidOperationException("End date is required.");

        if (request.EndedAt <= request.StartAt)
            throw new InvalidOperationException("End date must be later than start date.");

        if (!Enum.IsDefined(request.Type))
            throw new InvalidOperationException("Invalid activity type.");

        Event? ev = await _eventRepository.GetByIdAsync(request.EventId);

        if (ev is null)
            throw new NotFoundException("Event not found.");

        if (ev.OrganizerId != userId)
            throw new UnauthorizedException("Only the event organizer can create this activity.");

        if (request.StartAt < ev.StartAt)
            throw new InvalidOperationException("The activity cannot start before the event.");

        if (request.EndedAt > ev.EndedAt)
            throw new InvalidOperationException("The activity cannot end after the event.");

        var activity = new Activity
        {
            EventId = request.EventId,
            Title = title,
            Description = description,
            Type = request.Type,
            StartAt = request.StartAt.Value,
            EndedAt = request.EndedAt.Value,
            Location = location
        };

        Activity created = await _repository.CreateAsync(activity);
        return MapToResponse(created);
    }

    public async Task<ActivityResponse?> GetByIdAsync(int id)
    {
        Activity? activity = await _repository.GetByIdAsync(id);
        if (activity is null) return null;
        return MapToResponse(activity);
    }

    public async Task<List<ActivityResponse>> GetByEventIdAsync(int eventId)
    {
        Event? ev = await _eventRepository.GetByIdAsync(eventId);

        if (ev is null)
            throw new NotFoundException("Event not found.");

        List<Activity> activities = await _repository.GetByEventIdAsync(eventId);

        return activities
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<ActivityResponse> UpdateAsync(int activityId, UpdateActivityRequest request, int userId)
    {
        Activity? activity = await _repository.GetByIdAsync(activityId);

        if (activity is null)
            throw new NotFoundException("Activity not found.");

        Event? ev = await _eventRepository.GetByIdAsync(activity.EventId);

        if (ev is null)
            throw new NotFoundException("Event not found.");

        if (ev.OrganizerId != userId)
            throw new UnauthorizedException("Only the event organizer can edit this activity.");

        string title = (request.Title ?? string.Empty).Trim();
        string description = (request.Description ?? string.Empty).Trim();
        string location = (request.Location ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(title))
            throw new InvalidOperationException("Title is required.");

        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidOperationException("Description is required.");

        if (string.IsNullOrWhiteSpace(location))
            throw new InvalidOperationException("Location is required.");

        if (request.StartAt is null)
            throw new InvalidOperationException("Start date is required.");

        if (request.EndedAt is null)
            throw new InvalidOperationException("End date is required.");

        if (request.EndedAt <= request.StartAt)
            throw new InvalidOperationException("End date must be later than start date.");

        activity.Title = title;
        activity.Description = description;
        activity.Type = request.Type;
        activity.StartAt = request.StartAt.Value;
        activity.EndedAt = request.EndedAt.Value;
        activity.Location = location;

        await _repository.UpdateAsync(activity);

        return MapToResponse(activity);
    }

    public async Task DeleteAsync(int activityId, int userId)
    {
        Activity? activity = await _repository.GetByIdAsync(activityId);

        if (activity is null)
            throw new NotFoundException("Activity not found.");

        Event? ev = await _eventRepository.GetByIdAsync(activity.EventId);

        if (ev is null)
            throw new NotFoundException("Event not found.");

        if (ev.OrganizerId != userId)
            throw new UnauthorizedException("Only the event organizer can delete this activity.");

        await _repository.DeleteAsync(activity);
    }

    private static ActivityResponse MapToResponse(Activity activity)
    {
        return new ActivityResponse
        {
            Id = activity.Id,
            EventId = activity.EventId,
            Title = activity.Title,
            Description = activity.Description,
            Type = activity.Type,
            StartAt = activity.StartAt,
            EndedAt = activity.EndedAt,
            Location = activity.Location
        };
    }
}
