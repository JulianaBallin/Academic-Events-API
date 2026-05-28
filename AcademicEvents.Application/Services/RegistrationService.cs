using AcademicEvents.Application.DTOs.Registration;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;

namespace AcademicEvents.Application.Services;

/// <summary>
/// Service de inscricoes em eventos.
/// Valida duplicata antes de inserir mesmo com o indice unico no banco.
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
        // verifica na camada de servico antes de chegar no banco
        Registration? existente = await _repository.GetByUsuarioEEventoAsync(usuarioId, request.EventoId);
        if (existente is not null)
            throw new InscricaoDuplicadaException("Voce ja esta inscrito neste evento.");

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
        if (inscricao is null) throw new NotFoundException("Inscricao nao encontrada.");

        // so o proprio usuario pode cancelar a propria inscricao
        if (inscricao.UsuarioId != usuarioId)
            throw new UnauthorizedException("Voce so pode cancelar suas proprias inscricoes.");

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
