namespace AcademicEvents.Exceptions;

/// <summary>
/// Modelo simples usado nas respostas de erro da API.
/// Ajuda o cliente a receber sempre o mesmo formato quando uma regra falha.
/// </summary>
public class ErrorResponse
{
    public string Mensagem { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string Caminho { get; set; } = string.Empty;
    public DateTime DataHoraUtc { get; set; }
}
