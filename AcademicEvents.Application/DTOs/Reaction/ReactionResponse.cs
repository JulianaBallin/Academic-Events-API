namespace AcademicEvents.Application.DTOs.Reaction;

/// <summary>
/// Dados da reação retornados nos endpoints de leitura.
/// </summary>
public class ReactionResponse
{
    public int Id { get; set; }
    public int EventoId { get; set; }
    public int UsuarioId { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
}
