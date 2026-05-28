namespace AcademicEvents.Exceptions;

/// <summary>
/// Lançada quando o usuário autenticado tenta fazer uma ação
/// que não é permitida para ele, como editar o evento de outra pessoa.
/// O controller deve capturar e retornar 403.
/// </summary>
public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message) { }
}
