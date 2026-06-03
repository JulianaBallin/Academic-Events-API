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
            throw new InvalidOperationException("O título é obrigatório.");

        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidOperationException("A descrição é obrigatória.");

        if (string.IsNullOrWhiteSpace(location))
            throw new InvalidOperationException("O local é obrigatório.");

        if (request.StartAt is null)
            throw new InvalidOperationException("A data de início é obrigatória.");

        if (request.EndedAt is null)
            throw new InvalidOperationException("A data de fim é obrigatória.");

        if (request.EndedAt <= request.StartAt)
            throw new InvalidOperationException("A data de fim deve ser posterior à data de início.");

        if (!Enum.IsDefined(request.Type))
            throw new InvalidOperationException("Tipo de atividade inválido.");

        Event? ev = await _eventRepository.GetByIdAsync(request.EventId);

        if (ev is null)
            throw new NotFoundException("Evento não encontrado.");

        if (ev.OrganizerId != userId)
            throw new UnauthorizedException("Apenas o organizador do evento pode criar esta atividade.");

        if (request.StartAt < ev.StartAt)
            throw new InvalidOperationException("A atividade não pode iniciar antes do evento.");

        if (request.EndedAt > ev.EndedAt)
            throw new InvalidOperationException("A atividade não pode terminar após o evento.");

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
            throw new NotFoundException("Evento não encontrado.");

        List<Activity> activities = await _repository.GetByEventIdAsync(eventId);

        return activities
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<ActivityResponse> UpdateAsync(int activityId, UpdateActivityRequest request, int userId)
    {
        Activity? activity = await _repository.GetByIdAsync(activityId);

        if (activity is null)
            throw new NotFoundException("Atividade não encontrada.");

        Event? ev = await _eventRepository.GetByIdAsync(activity.EventId);

        if (ev is null)
            throw new NotFoundException("Evento não encontrado.");

        if (ev.OrganizerId != userId)
            throw new UnauthorizedException("Apenas o organizador do evento pode editar esta atividade.");

        string title = (request.Title ?? string.Empty).Trim();
        string description = (request.Description ?? string.Empty).Trim();
        string location = (request.Location ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(title))
            throw new InvalidOperationException("O título é obrigatório.");

        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidOperationException("A descrição é obrigatória.");

        if (string.IsNullOrWhiteSpace(location))
            throw new InvalidOperationException("O local é obrigatório.");

        if (request.StartAt is null)
            throw new InvalidOperationException("A data de início é obrigatória.");

        if (request.EndedAt is null)
            throw new InvalidOperationException("A data de fim é obrigatória.");

        if (request.EndedAt <= request.StartAt)
            throw new InvalidOperationException("A data de fim deve ser posterior à data de início.");

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
            throw new NotFoundException("Atividade não encontrada.");

        Event? ev = await _eventRepository.GetByIdAsync(activity.EventId);

        if (ev is null)
            throw new NotFoundException("Evento não encontrado.");

        if (ev.OrganizerId != userId)
            throw new UnauthorizedException("Apenas o organizador do evento pode excluir esta atividade.");

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
