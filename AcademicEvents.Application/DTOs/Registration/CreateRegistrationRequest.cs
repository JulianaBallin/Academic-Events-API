namespace AcademicEvents.Application.DTOs.Registration;

/// <summary>
/// Dados recebidos para inscrever o usuário em um evento.
/// </summary>
public class CreateRegistrationRequest
{
    public int EventoId { get; set; }
}
