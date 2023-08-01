namespace Acme.DrawLanding.Library.Domain.Submissions;

public sealed class Submission
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public Guid SerialNumber { get; set; }

    public DateTime SubmittedAt { get; set; }
}
