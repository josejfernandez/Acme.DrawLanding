namespace Acme.DrawLanding.Library.Domain.Submissions;

public sealed class SubmissionInsertionResult
{
    public static SubmissionInsertionResult AlreadyUsed = new(0);
    public static SubmissionInsertionResult DoesNotExist = new(0);

    public int RemainingUses { get; }

    private SubmissionInsertionResult(int remainingUses)
    {
        RemainingUses = remainingUses;
    }

    public static SubmissionInsertionResult Ok(int remainingUses)
    {
        if (remainingUses < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(remainingUses));
        }

        return new SubmissionInsertionResult(remainingUses);
    }
}
