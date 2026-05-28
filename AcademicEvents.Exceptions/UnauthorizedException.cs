namespace AcademicEvents.Exceptions;

/// <summary>
/// Lancada quando o usuario autenticado tenta fazer uma acao
/// que nao e permitida para ele, como editar o evento de outra pessoa.
/// O controller deve capturar e retornar 403.
/// </summary>
public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message) { }
}
