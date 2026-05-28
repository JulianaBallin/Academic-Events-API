namespace AcademicEvents.Exceptions;

/// <summary>
/// Lançada quando email ou senha estão incorretos no login.
/// Retorna 401. Não especifica qual dos dois está errado por segurança.
/// </summary>
public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(string message) : base(message) { }
}
