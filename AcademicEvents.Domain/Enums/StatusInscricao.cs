namespace AcademicEvents.Domain.Enums;

/// <summary>
/// Estado da inscrição de um usuário em um evento.
/// Pendente é o estado inicial; a confirmação pode ser manual pelo organizador.
/// </summary>
public enum StatusInscricao
{
    Pendente,
    Confirmada,
    Cancelada
}
