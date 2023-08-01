using System.ComponentModel.DataAnnotations;

namespace Acme.DrawLanding.Website.Domain.Submissions;

public sealed class FormSubmissionRequest
{
    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public Guid? SerialNumber { get; set; }

    [Required]
    public bool IsAdult { get; set; }
}
