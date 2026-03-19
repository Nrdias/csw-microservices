using System.ComponentModel.DataAnnotations;

namespace EnrollmentService.Models;

public class Enrollment
{
    public Guid Id { get; set; }

    [Required]
    public string StudentRegistrationNumber { get; set; } = string.Empty;

    [Required]
    public string DisciplineCode { get; set; } = string.Empty;

    [Required]
    public char Schedule { get; set; }
}

public class EnrollmentRequest
{
    [Required]
    public string StudentRegistrationNumber { get; set; } = string.Empty;

    [Required]
    public string DisciplineCode { get; set; } = string.Empty;

    [Required]
    public char Schedule { get; set; }
}