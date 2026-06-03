using AcademicEvents.Application.DTOs.Registration;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;

namespace AcademicEvents.Application.Services;

/// <summary>
/// Event registration service.
/// </summary>
public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationRepository _repository;
    private readonly IEventRepository _eventRepository;

    public RegistrationService(IRegistrationRepository repository, IEventRepository eventRepository)
    {
        _repository = repository;
        _eventRepository = eventRepository;
    }

    public async Task<RegistrationResponse> CreateAsync(CreateRegistrationRequest request, int userId)
    {
        if (request.EventId <= 0)
            throw new InvalidOperationException("Event id must be greater than zero.");

        Event? eventRepository = await _eventRepository.GetByIdAsync(request.EventId);
        if (eventRepository is null)
            throw new NotFoundException("Event not found.");

        // Checks event on service layer, before the database
        Registration? searchEventResponse = await _repository.GetByUserEventAsync(userId, request.EventId);
        if (searchEventResponse is not null)
            throw new DuplicateRegistrationException("The user is already registered for this event.");

        Registration registration = new Registration
        {
            EventId = request.EventId,
            UserId = userId
        };

        Registration createdEvent = await _repository.CreateAsync(registration);
        return MapResponse(createdEvent);
    }

    public async Task<List<RegistrationResponse>> GetByUserAsync(int userId)
    {
        List<Registration> registrations = await _repository.GetByUserAsync(userId);
        return registrations.Select(MapResponse).ToList();
    }

    public async Task DeleteAsync(int id, int userId)
    {
        Registration? registration = await _repository.GetByIdAsync(id);
        if (registration is null) throw new NotFoundException("Registration not found.");

        // Only subscribed user may cancel its event subscription 
        if (registration.UserId != userId)
            throw new UnauthorizedException("Cannot cancel another user's registration.");

        await _repository.DeleteAsync(id);
    }

    private static RegistrationResponse MapResponse(Registration registration)
    {
        return new RegistrationResponse
        {
            Id = registration.Id,
            EventId = registration.EventId,
            EventTitle = registration.Event?.Title ?? string.Empty,
            UserId = registration.UserId,
            UserName = registration.User?.Name ?? string.Empty,
            Status = registration.Status.ToString(),
            CreatedAt = registration.CreatedAt
        };
    }
}
