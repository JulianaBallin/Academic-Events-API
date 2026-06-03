using AcademicEvents.Application.DTOs.Activity;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;


namespace AcademicEvents.Application.Services;

/// <summary>
/// Service deatividades de eventos acadêmicos.
/// </summary>
public class ActivityService : IActivityService
{
    private readonly IActivityRepository _repository;
    private readonly IEventRepository _eventRepository;

    public ActivityService(IActivityRepository repository,IEventRepository eventRepository)
    {
        _repository = repository;
        _eventRepository = eventRepository;
    }

    public async Task<ActivityResponse> CreateAsync(CreateActivityRequest request, int usuarioId)
    {

        string titulo = (request.Titulo ?? string.Empty).Trim();
        string descricao = (request.Descricao ?? string.Empty).Trim();
        string local = (request.Local ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(titulo))
            throw new InvalidOperationException("O título é obrigatório.");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new InvalidOperationException("A descrição é obrigatória.");

        if (string.IsNullOrWhiteSpace(local))
            throw new InvalidOperationException("O local é obrigatório.");

        if (request.DataFim <= request.DataInicio)
            throw new InvalidOperationException("A data de fim deve ser posterior à data de início.");

        if (!Enum.IsDefined(request.Tipo))
            throw new InvalidOperationException("Tipo de atividade inválido.");


        Event? evento = await _eventRepository.GetByIdAsync(request.EventId);

        if (evento is null)
            throw new NotFoundException("Evento não encontrado.");

        if (evento.OrganizerId != usuarioId)
            throw new UnauthorizedException("Apenas o organizador do evento pode criar esta atividade.");

        if (request.DataInicio < evento.StartAt)
            throw new InvalidOperationException(
                "A atividade não pode iniciar antes do evento.");

        if (request.DataFim > evento.EndedAt)
            throw new InvalidOperationException(
                "A atividade não pode terminar após o evento.");    

        var activity = new Activity
        {
            EventId = request.EventId,
            Titulo = titulo,
            Descricao = descricao,
            Tipo = request.Tipo,
            DataInicio = request.DataInicio,
            DataFim = request.DataFim,
            Local = local
        };

        Activity criada = await _repository.CreateAsync(activity);
        return MapearParaResponse(criada);
    }

    public async Task<ActivityResponse?> GetByIdAsync(int id)
    {
        Activity? activity = await _repository.GetByIdAsync(id);
        if (activity is null) return null;
        return MapearParaResponse(activity);
    }

    public async Task<List<ActivityResponse>> GetByEventIdAsync(int eventId)
    {
        Event? evento = await _eventRepository.GetByIdAsync(eventId);

        if (evento is null)
            throw new NotFoundException("Evento não encontrado.");
        
        List<Activity> activities = await _repository.GetByEventIdAsync(eventId);

        return activities
            .Select(MapearParaResponse)
            .ToList();
    }

    public async Task<ActivityResponse> UpdateAsync(int activityId,UpdateActivityRequest request, int usuarioId)
    {
        Activity? activity = await _repository.GetByIdAsync(activityId);

        if (activity is null)
            throw new NotFoundException("Atividade não encontrada.");

        Event? evento = await _eventRepository.GetByIdAsync(activity.EventId);

        if (evento is null)
            throw new NotFoundException("Evento não encontrado.");

        if (evento.OrganizerId != usuarioId)
            throw new UnauthorizedException("Apenas o organizador do evento pode editar esta atividade.");

        string titulo = (request.Titulo ?? string.Empty).Trim();
        string descricao = (request.Descricao ?? string.Empty).Trim();
        string local = (request.Local ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(titulo))
            throw new InvalidOperationException(
                "O título é obrigatório.");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new InvalidOperationException(
                "A descrição é obrigatória.");

        if (string.IsNullOrWhiteSpace(local))
            throw new InvalidOperationException(
                "O local é obrigatório.");

        if (request.DataFim <= request.DataInicio)
            throw new InvalidOperationException(
                "A data de fim deve ser posterior à data de início.");

        activity.Titulo = titulo;
        activity.Descricao = descricao;
        activity.Tipo = request.Tipo;
        activity.DataInicio = request.DataInicio;
        activity.DataFim = request.DataFim;
        activity.Local = local;

        await _repository.UpdateAsync(activity);

        return MapearParaResponse(activity);
    }

    public async Task DeleteAsync(int activityId,int usuarioId)
    {
        Activity? activity = await _repository.GetByIdAsync(activityId);

        if (activity is null)
            throw new NotFoundException("Atividade não encontrada.");

        Event? evento = await _eventRepository.GetByIdAsync(activity.EventId);

        if (evento is null)
            throw new NotFoundException("Evento não encontrado.");

        if (evento.OrganizerId != usuarioId)
            throw new UnauthorizedException("Apenas o organizador do evento pode excluir esta atividade.");

        await _repository.DeleteAsync(activity);
    }

    private static ActivityResponse MapearParaResponse(Activity atividade)
    {
        return new ActivityResponse
        {
            Id = atividade.Id,
            EventId = atividade.EventId,
            Titulo = atividade.Titulo,
            Descricao = atividade.Descricao,
            Tipo = atividade.Tipo,
            DataInicio = atividade.DataInicio,
            DataFim = atividade.DataFim,
            Local = atividade.Local
        };
    }
}


