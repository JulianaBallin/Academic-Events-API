using AcademicEvents.Application.DTOs.Event;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Enums;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;

namespace AcademicEvents.Application.Services;

/// <summary>
/// Service de eventos academicos.
/// Cuida de criar, buscar, atualizar e remover eventos.
/// </summary>
public class EventService : IEventService
{
    private readonly IEventRepository _repository;

    public EventService(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<EventResponse> CreateAsync(CreateEventRequest request, int organizadorId)
    {
        if (request.DataFim <= request.DataInicio)
            throw new InvalidOperationException("A data de fim deve ser posterior a data de inicio.");

        Event evento = new Event
        {
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            DataInicio = request.DataInicio,
            DataFim = request.DataFim,
            Local = request.Local,
            OrganizadorId = organizadorId
        };

        Event criado = await _repository.CreateAsync(evento);
        return MapearParaResponse(criado);
    }

    public async Task<EventResponse?> GetByIdAsync(int id)
    {
        Event? evento = await _repository.GetByIdAsync(id);
        if (evento is null) return null;
        return MapearParaResponse(evento);
    }

    public async Task<List<EventResponse>> GetAllAsync(string? status)
    {
        // sem filtro retorna todos os eventos
        if (string.IsNullOrEmpty(status))
        {
            List<Event> todos = await _repository.GetAllAsync();
            return todos.Select(MapearParaResponse).ToList();
        }

        // tenta converter o status recebido como string para o enum
        if (!Enum.TryParse<StatusEvento>(status, ignoreCase: true, out StatusEvento statusEnum))
            throw new InvalidOperationException($"Status '{status}' invalido.");

        List<Event> filtrados = await _repository.GetByStatusAsync(statusEnum);
        return filtrados.Select(MapearParaResponse).ToList();
    }

    public async Task<EventResponse?> UpdateAsync(int id, UpdateEventRequest request, int usuarioId)
    {
        Event? evento = await _repository.GetByIdAsync(id);
        if (evento is null) throw new NotFoundException("Evento nao encontrado.");

        // so o organizador pode editar o proprio evento
        if (evento.OrganizadorId != usuarioId)
            throw new UnauthorizedException("Apenas o organizador pode editar este evento.");

        evento.Titulo = request.Titulo;
        evento.Descricao = request.Descricao;
        evento.DataInicio = request.DataInicio;
        evento.DataFim = request.DataFim;
        evento.Local = request.Local;
        evento.Status = request.Status;

        Event? atualizado = await _repository.UpdateAsync(evento);
        return atualizado is null ? null : MapearParaResponse(atualizado);
    }

    public async Task DeleteAsync(int id, int usuarioId)
    {
        Event? evento = await _repository.GetByIdAsync(id);
        if (evento is null) throw new NotFoundException("Evento nao encontrado.");

        if (evento.OrganizadorId != usuarioId)
            throw new UnauthorizedException("Apenas o organizador pode remover este evento.");

        await _repository.DeleteAsync(id);
    }

    private static EventResponse MapearParaResponse(Event evento)
    {
        return new EventResponse
        {
            Id = evento.Id,
            Titulo = evento.Titulo,
            Descricao = evento.Descricao,
            DataInicio = evento.DataInicio,
            DataFim = evento.DataFim,
            Local = evento.Local,
            Status = evento.Status.ToString(),
            OrganizadorId = evento.OrganizadorId,
            NomeOrganizador = evento.Organizador?.Nome ?? string.Empty,
            CriadoEm = evento.CriadoEm
        };
    }
}
