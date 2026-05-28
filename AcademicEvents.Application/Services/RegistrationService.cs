using AcademicEvents.Application.DTOs.Registration;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;

namespace AcademicEvents.Application.Services;

/// <summary>
/// Service de inscrições em eventos.
/// Valida duplicata antes de inserir mesmo com o índice único no banco.
/// </summary>
public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationRepository _repository;

    public RegistrationService(IRegistrationRepository repository)
    {
        _repository = repository;
    }

    public async Task<RegistrationResponse> CreateAsync(CreateRegistrationRequest request, int usuarioId)
    {
        // verifica na camada de serviço antes de chegar no banco
        Registration? existente = await _repository.GetByUsuarioEEventoAsync(usuarioId, request.EventoId);
        if (existente is not null)
            throw new InscricaoDuplicadaException("Você já está inscrito neste evento.");

        Registration inscricao = new Registration
        {
            EventoId = request.EventoId,
            UsuarioId = usuarioId
        };

        Registration criada = await _repository.CreateAsync(inscricao);
        return MapearParaResponse(criada);
    }

    public async Task<List<RegistrationResponse>> GetByUsuarioAsync(int usuarioId)
    {
        List<Registration> inscricoes = await _repository.GetByUsuarioAsync(usuarioId);
        return inscricoes.Select(MapearParaResponse).ToList();
    }

    public async Task DeleteAsync(int id, int usuarioId)
    {
        Registration? inscricao = await _repository.GetByIdAsync(id);
        if (inscricao is null) throw new NotFoundException("Inscrição não encontrada.");

        // só o próprio usuário pode cancelar a própria inscrição
        if (inscricao.UsuarioId != usuarioId)
            throw new UnauthorizedException("Você só pode cancelar suas próprias inscrições.");

        await _repository.DeleteAsync(id);
    }

    private static RegistrationResponse MapearParaResponse(Registration inscricao)
    {
        return new RegistrationResponse
        {
            Id = inscricao.Id,
            EventoId = inscricao.EventoId,
            TituloEvento = inscricao.Evento?.Titulo ?? string.Empty,
            UsuarioId = inscricao.UsuarioId,
            NomeUsuario = inscricao.Usuario?.Nome ?? string.Empty,
            Status = inscricao.Status.ToString(),
            CriadoEm = inscricao.CriadoEm
        };
    }
}
