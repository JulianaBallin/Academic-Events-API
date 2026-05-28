namespace AcademicEvents.Exceptions;

/// <summary>
/// Lancada quando o usuario tenta se inscrever num evento em que ja esta inscrito.
/// O controller deve capturar e retornar 400.
/// </summary>
public class InscricaoDuplicadaException : Exception
{
    public InscricaoDuplicadaException(string message) : base(message) { }
}
