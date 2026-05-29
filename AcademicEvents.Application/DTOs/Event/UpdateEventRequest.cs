using System.ComponentModel.DataAnnotations;
using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Application.DTOs.Event;

/// <summary>
/// Dados recebidos para atualizar um evento existente.
/// Só o organizador pode chamar esse endpoint.
/// </summary>
public class UpdateEventRequest
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 2000 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de início é obrigatória.")]
    public DateTime DataInicio { get; set; }

    [Required(ErrorMessage = "A data de fim é obrigatória.")]
    public DateTime DataFim { get; set; }

    [Required(ErrorMessage = "O local é obrigatório.")]
    [StringLength(300, MinimumLength = 3, ErrorMessage = "O local deve ter entre 3 e 300 caracteres.")]
    public string Local { get; set; } = string.Empty;

    [Required(ErrorMessage = "O status é obrigatório.")]
    public StatusEvento Status { get; set; }
}
