namespace AcademicEvents.Exceptions;

/// <summary>
/// Lancada quando email ou senha estao incorretos no login.
/// Retorna 401. Nao especifica qual dos dois esta errado por seguranca.
/// </summary>
public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(string message) : base(message) { }
}
