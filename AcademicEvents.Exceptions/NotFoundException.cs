namespace AcademicEvents.Exceptions;

/// <summary>
/// Lançada quando um recurso solicitado não existe no banco.
/// O controller deve capturar e retornar 404.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
