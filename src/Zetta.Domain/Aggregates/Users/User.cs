using Zetta.Domain.Events;
using Zetta.Domain.ValueObjects;
using Zetta.SharedKernel.Primitives;
using Zetta.SharedKernel.Results;

namespace Zetta.Domain.Aggregates.Users;

public sealed class User : AggregateRoot
{
    private User(Guid id, string name, Email email, string passwordHash)
        : base(id)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
    }

    private User() { }

    public string Name { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = string.Empty;

    public static Result<User> Create(string name, Email email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<User>(Error.Validation("Name cannot be empty."));

        var user = new User(Guid.NewGuid(), name.Trim(), email, passwordHash);
        user.RaiseDomainEvent(new UserCreatedEvent(user.Id, email.Value));

        return user;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return;

        Name = name.Trim();
        SetUpdatedAt();
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        SetUpdatedAt();
    }
}
