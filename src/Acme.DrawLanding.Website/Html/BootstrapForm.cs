using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Acme.DrawLanding.Website.Html;

public sealed class BootstrapForm
{
    public string ValidationClass(ModelStateEntry? model, bool isAspValidationSpan = false)
    {
        if (model == null)
        {
            return string.Empty;
        }

        switch (model.ValidationState)
        {
            case ModelValidationState.Skipped:
            case ModelValidationState.Unvalidated:
                return string.Empty;

            case ModelValidationState.Invalid:
                return isAspValidationSpan ? "invalid-feedback" : "is-invalid";

            case ModelValidationState.Valid:
                return isAspValidationSpan ? "valid-feedback" : "is-valid";

            default:
                throw new NotSupportedException();
        }
    }
}
