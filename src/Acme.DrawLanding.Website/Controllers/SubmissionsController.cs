using Acme.DrawLanding.Library.Domain.Submissions;
using Acme.DrawLanding.Website.Domain.Submissions;
using Microsoft.AspNetCore.Mvc;

namespace Acme.DrawLanding.Website.Controllers;

[ApiController]
public sealed class SubmissionsController : Controller
{
    private readonly ISubmissionRepository _submissionRepository;

    public SubmissionsController(ISubmissionRepository submissionRepository)
    {
        _submissionRepository = submissionRepository ?? throw new ArgumentNullException(nameof(submissionRepository));
    }

    [HttpPost("/submit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index([FromBody] FormSubmissionRequest request)
    {
        if (!request.IsAdult)
        {
            ModelState.AddModelError(
                nameof(FormSubmissionRequest.IsAdult),
                Messages.OnlyAdultsAreAllowedToParticipate);

            return ValidationProblem(ModelState);
        }

        var submission = CreateSubmissionFromRequest(request);

        var insertionResult = await _submissionRepository.InsertIfNotUsedMoreThan(submission, Constants.MaxNumberOfUsesPerSerialNumber);

        if (insertionResult == SubmissionInsertionResult.DoesNotExist)
        {
            ModelState.AddModelError(
                nameof(FormSubmissionRequest.SerialNumber),
                Messages.SerialNumberDoesNotExist);

            return ValidationProblem(ModelState);
        }
        else if (insertionResult == SubmissionInsertionResult.AlreadyUsed)
        {
            ModelState.AddModelError(
                nameof(FormSubmissionRequest.SerialNumber),
                Messages.SerialNumberHasBeenUsedTooManyTimes);

            return ValidationProblem(ModelState);
        }
        else
        {
            return Ok(new RemainingUsesResponse()
            {
                RemainingUses = insertionResult.RemainingUses,
            });
        }
    }

    private static Submission CreateSubmissionFromRequest(FormSubmissionRequest request)
    {
        return new Submission()
        {
            FirstName = request.FirstName!,
            LastName = request.LastName!,
            Email = request.Email!,
            SerialNumber = request.SerialNumber!.Value,
        };
    }
}