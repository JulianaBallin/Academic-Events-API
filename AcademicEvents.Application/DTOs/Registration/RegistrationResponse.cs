namespace AcademicEvents.Application.DTOs.Registration;

/// <summary>
/// Dados da inscrição retornados nos endpoints de leitura.
/// </summary>
public class RegistrationResponse
{
    public int Id { get; set; }
    public int EventoId { get; set; }
    public string TituloEvento { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
}
