using System.ComponentModel.DataAnnotations;

namespace StudentService.Models;

public class Student
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string RegistrationNumber { get; set; } = string.Empty;
}