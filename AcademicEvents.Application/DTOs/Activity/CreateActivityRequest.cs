using AcademicEvents.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AcademicEvents.Application.DTOs.Activity;

/// <summary>
/// Data received at the activity creation endpoint.
/// </summary>
public class CreateActivityRequest
{
    [Required(ErrorMessage = "O id do evento é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O id do evento deve ser maior que zero.")]
    public int EventId { get; set; }

    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 200 caracteres.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 2000 caracteres.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo de atividade é obrigatório.")]
    [EnumDataType(typeof(ActivityType), ErrorMessage = "Tipo de atividade inválido.")]
    public ActivityType Type { get; set; }

    [Required(ErrorMessage = "A data de início é obrigatória.")]
    public DateTime StartAt { get; set; }

    [Required(ErrorMessage = "A data de fim é obrigatória.")]
    public DateTime EndedAt { get; set; }

    [Required(ErrorMessage = "O local é obrigatório.")]
    [StringLength(300, MinimumLength = 3, ErrorMessage = "O local deve ter entre 3 e 300 caracteres.")]
    public string Location { get; set; } = string.Empty;
}
