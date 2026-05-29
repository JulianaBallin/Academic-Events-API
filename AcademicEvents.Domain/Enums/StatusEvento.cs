namespace AcademicEvents.Domain.Enums;

/// <summary>
/// Ciclo de vida de um evento acadêmico.
/// Rascunho é o estado inicial ao criar; Publicado torna o evento visível para inscrições.
/// </summary>
public enum StatusEvento
{
    Rascunho,
    Publicado,
    Cancelado,
    Concluido
}
