using Acme.DrawLanding.Website.Domain.Submissions;

namespace Acme.DrawLanding.Tests.Mocking.Submissions;

public static class SubmissionMocking
{
    public static FormSubmissionRequest CreateValidSubmissionRequest()
    {
        return new FormSubmissionRequest()
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "example@example.com",
            SerialNumber = Guid.NewGuid(),
            IsAdult = true,
        };
    }
}
