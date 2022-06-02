using Checkin.Common.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace Checkin.Common.Validation.Extensions;

public static class FluentValidatorExtensions
{
    public static async Task ValidateThrowAsync<T>(this AbstractValidator<T> validator, T instance,
        CancellationToken cancellationToken = default)
    {
        var result = await validator.ValidateAsync(instance, cancellationToken);

        if (result.IsValid)
            return;

        CheckinException.Throw(FormatErrorMessage(result.Errors), 400);
    }

    private static string FormatErrorMessage(ICollection<ValidationFailure> errors)
    {
        return errors.Count is 0
            ? string.Empty
            : string.Join("; ", errors.Select(e => $"{e}"));
    }
}