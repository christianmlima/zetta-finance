using FluentValidation;
using Zetta.Domain.Enums;

namespace Zetta.Application.Transactions.Commands.CreateTransaction;

public sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Description).NotEmpty().MaximumLength(255);

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .When(x => x.Type != TransactionType.Transfer)
            .WithMessage("Category is required for income and expense transactions.");

        RuleFor(x => x.TransferTargetAccountId)
            .NotEmpty()
            .When(x => x.Type == TransactionType.Transfer)
            .WithMessage("Transfer requires a target account.");
    }
}
