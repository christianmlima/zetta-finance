using System.Text.RegularExpressions;
using Zetta.SharedKernel.Results;

namespace Zetta.Domain.ValueObjects;

public sealed record Email
{
    private static readonly Regex EmailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    private Email(string value) => Value = value;

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Email>(Error.Validation("Email cannot be empty."));

        if (!EmailRegex.IsMatch(value))
            return Result.Failure<Email>(Error.Validation("Email format is invalid."));

        return Result.Success(new Email(value.ToLowerInvariant()));
    }

    public override string ToString() => Value;
}
