using AcademicEvents.Application.DTOs.Event;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Enums;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;

namespace AcademicEvents.Application.Services;

/// <summary>
/// Academic event service.
/// Handles creating, searching, updating, and deleting events.
/// </summary>
public class EventService : IEventService
{
    private readonly IEventRepository _repository;

    public EventService(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<EventResponse> CreateAsync(CreateEventRequest request, int organizerId)
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

        if (request.EndDate <= request.StartDate)
            throw new InvalidOperationException("End date should be after the start date.");

        Event newEvent = new Event
        {
            Title = title,
            Description = description,
            StartAt = request.StartDate,
            EndedAt = request.EndDate,
            Location = location,
            OrganizerId = organizerId
        };

        Event eventEntity = await _repository.CreateAsync(newEvent);
        return MapResponse(eventEntity);
    }

    public async Task<EventResponse?> GetByIdAsync(int id)
    {
        Event? eventEntity = await _repository.GetByIdAsync(id);
        if (eventEntity is null) return null;
        return MapResponse(eventEntity);
    }

    public async Task<List<EventResponse>> GetAllAsync(string? status, int? organizerId)
    {
        if (organizerId is <= 0)
            throw new InvalidOperationException("Organizer id must be greater than zero.");

        EventStatus? statusEnum = null;

        // Attempts to convert the received status string to the enum.
        if (!string.IsNullOrWhiteSpace(status))
        {
            string normalizedStatus = status.Trim();

            if (!Enum.TryParse<EventStatus>(normalizedStatus, ignoreCase: true, out EventStatus parsedStatus))
                throw new InvalidOperationException($"Invalid status '{status}'.");

            if (!Enum.IsDefined(parsedStatus))
                throw new InvalidOperationException($"Invalid status '{status}'.");

            statusEnum = parsedStatus;
        }

        List<Event> filteredEvent = await _repository.GetFilteredAsync(statusEnum, organizerId);
        return filteredEvent.Select(MapResponse).ToList();
    }

    public async Task<List<EventResponse>> GetByOrganizerAsync(int organizerId)
    {
        List<Event> eventsByOrganizer = await _repository.GetByOrganizerAsync(organizerId);
        return eventsByOrganizer.Select(MapResponse).ToList();
    }

    public async Task<EventResponse?> UpdateAsync(int id, UpdateEventRequest request, int userId)
    {
        string title = (request.Title ?? string.Empty).Trim();
        string description = (request.Description ?? string.Empty).Trim();
        string location = (request.Location ?? string.Empty).Trim();

        Event? eventEntity = await _repository.GetByIdAsync(id);
        if (eventEntity is null) throw new NotFoundException("Event not found.");

        // only the organizer may edit its own event
        if (eventEntity.OrganizerId != userId)
            throw new UnauthorizedException("Only the organizer can edit this event.");

        if (request.EndDate <= request.StartDate)
            throw new InvalidOperationException("End date must be later than start date.");

        if (string.IsNullOrWhiteSpace(title))
            throw new InvalidOperationException("Title is required.");

        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidOperationException("Description is required.");

        if (string.IsNullOrWhiteSpace(location))
            throw new InvalidOperationException("Location is required.");

        if (!Enum.IsDefined(request.EventStatus))
            throw new InvalidOperationException("Invalid status.");

        eventEntity.Title = title;
        eventEntity.Description = description;
        eventEntity.StartAt = request.StartDate;
        eventEntity.EndedAt = request.EndDate;
        eventEntity.Location = location;
        eventEntity.EventStatus = request.EventStatus;

        Event? updatedEvent = await _repository.UpdateAsync(eventEntity);
        return updatedEvent is null ? null : MapResponse(updatedEvent);
    }

    public async Task DeleteAsync(int id, int userId)
    {
        Event? eventEntity = await _repository.GetByIdAsync(id);
        if (eventEntity is null) throw new NotFoundException("Event not found.");

        if (eventEntity.OrganizerId != userId)
            throw new UnauthorizedException("Only the organizer can delete this event.");

        await _repository.DeleteAsync(id);
    }

    private static EventResponse MapResponse(Event eventEntity)
    {
        return new EventResponse
        {
            Id = eventEntity.Id,
            Title = eventEntity.Title,
            Description = eventEntity.Description,
            StartDate = eventEntity.StartAt,
            EndDate = eventEntity.EndedAt,
            Location = eventEntity.Location,
            Status = eventEntity.EventStatus.ToString(),
            OrganizerId = eventEntity.OrganizerId,
            OrganizerName = eventEntity.Organizer?.Name ?? string.Empty,
            CreatedAt = eventEntity.CreatedAt
        };
    }
}
