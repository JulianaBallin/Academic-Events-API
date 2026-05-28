namespace AcademicEvents.Exceptions;

/// <summary>
/// Lancada quando se tenta cadastrar um email que ja existe no banco.
/// O controller deve capturar e retornar 400.
/// </summary>
public class DuplicateEmailException : Exception
{
    public DuplicateEmailException(string message) : base(message) { }
}
