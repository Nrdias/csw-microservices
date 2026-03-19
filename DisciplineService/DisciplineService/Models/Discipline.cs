using System.ComponentModel.DataAnnotations;

namespace DisciplineService.Models;

public class Discipline
{
    public Guid Id { get; set; }

    [Required]
    public string Code { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public char Schedule { get; set; }
}