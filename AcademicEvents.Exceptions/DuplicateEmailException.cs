namespace AcademicEvents.Exceptions;

/// <summary>
/// Lançada quando se tenta cadastrar um email que já existe no banco.
/// O controller deve capturar e retornar 400.
/// </summary>
public class DuplicateEmailException : Exception
{
    public DuplicateEmailException(string message) : base(message) { }
}
