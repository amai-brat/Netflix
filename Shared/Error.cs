namespace Shared;

public record Error
{
    public static readonly Error None = new(string.Empty, ErrorType.Failure);
    public static readonly Error NullValue = new("Null value was provided", ErrorType.Failure);

    private Error(string description, ErrorType errorType)
    {
        Description = description;
        Type = errorType;
    }

    public string Description { get; }

    public ErrorType Type { get; }

    public static Error NotFound(string description) =>
        new(description, ErrorType.NotFound);

    public static Error Validation(string description) =>
        new(description, ErrorType.Validation);

    public static Error Conflict(string description) =>
        new(description, ErrorType.Conflict);

    public static Error Failure(string description) =>
        new(description, ErrorType.Failure);
}
