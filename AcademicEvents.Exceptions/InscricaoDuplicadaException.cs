namespace AcademicEvents.Exceptions;

/// <summary>
/// Lançada quando o usuário tenta se inscrever num evento em que já está inscrito.
/// O controller deve capturar e retornar 400.
/// </summary>
public class InscricaoDuplicadaException : Exception
{
    public InscricaoDuplicadaException(string message) : base(message) { }
}
