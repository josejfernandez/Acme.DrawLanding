using Acme.DrawLanding.Library.Domain.SerialNumbers;

namespace Acme.DrawLanding.Library.Domain.Submissions;

public sealed class SubmissionRecord
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime SubmittedAt { get; set; }

    public int SerialNumberId { get; set; }

    public SerialNumberRecord SerialNumber { get; set; } = null!;
}
