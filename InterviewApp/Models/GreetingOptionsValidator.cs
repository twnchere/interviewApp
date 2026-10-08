#nullable enable

using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace InterviewApp.Models;

public sealed class GreetingOptionsValidator : IValidateOptions<GreetingOptions>
{
    public ValidateOptionsResult Validate(string? name, GreetingOptions options)
    {
        List<string> failures = new();

        if (string.IsNullOrWhiteSpace(options.Message))
        {
            failures.Add("Greeting:Message must not be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(options.Language))
        {
            failures.Add("Greeting:Language must not be null or empty.");
        }

        return failures.Count > 0
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;
    }
}
