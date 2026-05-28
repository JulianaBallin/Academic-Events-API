namespace AcademicEvents.Exceptions;

/// <summary>
/// Lancada quando um recurso solicitado nao existe no banco.
/// O controller deve capturar e retornar 404.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
