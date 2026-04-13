using System.Security.Claims;
using MediatR;
using Zetta.Api.Extensions;
using Zetta.Application.Transactions.Commands.CreateTransaction;
using Zetta.Application.Transactions.Commands.DeleteTransaction;
using Zetta.Application.Transactions.Queries.GetTransactionById;
using Zetta.Application.Transactions.Queries.GetTransactions;
using Zetta.Domain.Enums;

namespace Zetta.Api.Endpoints;

public static class TransactionEndpoints
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/transactions").WithTags("Transactions").RequireAuthorization();

        group.MapGet("/", async (
            ClaimsPrincipal user,
            ISender sender,
            int page = 1,
            int pageSize = 20,
            DateOnly? from = null,
            DateOnly? to = null,
            TransactionType? type = null,
            Guid? categoryId = null,
            Guid? accountId = null) =>
        {
            var userId = user.GetUserId();
            var query = new GetTransactionsQuery(userId, page, pageSize, from, to, type, categoryId, accountId);
            var result = await sender.Send(query);
            return result.ToHttpResult();
        });

        group.MapGet("/{id:guid}", async (Guid id, ClaimsPrincipal user, ISender sender) =>
        {
            var userId = user.GetUserId();
            var result = await sender.Send(new GetTransactionByIdQuery(id, userId));
            return result.ToHttpResult();
        });

        group.MapPost("/", async (CreateTransactionRequest request, ClaimsPrincipal user, ISender sender) =>
        {
            var userId = user.GetUserId();
            var command = new CreateTransactionCommand(
                userId,
                request.AccountId,
                request.Type,
                request.Amount,
                request.Date,
                request.Description,
                request.CategoryId,
                request.TransferTargetAccountId);
            var result = await sender.Send(command);
            return result.ToCreatedResult($"/transactions/{(result.IsSuccess ? result.Value.Id : Guid.Empty)}");
        });

        group.MapDelete("/{id:guid}", async (Guid id, ClaimsPrincipal user, ISender sender) =>
        {
            var userId = user.GetUserId();
            var result = await sender.Send(new DeleteTransactionCommand(id, userId));
            return result.ToHttpResult();
        });
    }

    private sealed record CreateTransactionRequest(
        Guid AccountId,
        TransactionType Type,
        decimal Amount,
        DateOnly Date,
        string Description,
        Guid? CategoryId = null,
        Guid? TransferTargetAccountId = null);
}
