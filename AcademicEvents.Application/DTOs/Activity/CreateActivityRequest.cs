using AcademicEvents.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AcademicEvents.Application.DTOs.Activity;

/// <summary>
/// Dados recebidos no endpoint de cadastro de uma nova atividade.
/// </summary>
public class CreateActivityRequest
{   
    [Required(ErrorMessage = "O id do evento é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O id do evento deve ser maior que zero.")]
    public int EventId { get; set; }

    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 2000 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo de atividade é obrigatório.")]
    [EnumDataType(typeof(TipoAtividade), ErrorMessage = "Tipo de atividade inválido.")]
    public TipoAtividade Tipo { get; set; }

    [Required(ErrorMessage = "A data de início é obrigatória.")] 
    public DateTime DataInicio { get; set; }

    [Required(ErrorMessage = "A data de fim é obrigatória.")]
    public DateTime DataFim { get; set; }

    [Required(ErrorMessage = "O local é obrigatório.")]
    [StringLength(300, MinimumLength = 3, ErrorMessage = "O local deve ter entre 3 e 300 caracteres.")]
    public string Local { get; set; } = string.Empty;
}