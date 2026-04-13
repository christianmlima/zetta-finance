using System.Security.Claims;
using MediatR;
using Zetta.Api.Extensions;
using Zetta.Application.Accounts.Commands.CreateAccount;
using Zetta.Application.Accounts.Commands.DeleteAccount;
using Zetta.Application.Accounts.Commands.UpdateAccount;
using Zetta.Application.Accounts.Queries.GetAccountById;
using Zetta.Application.Accounts.Queries.GetAccounts;
using Zetta.Domain.Enums;

namespace Zetta.Api.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/accounts").WithTags("Accounts").RequireAuthorization();

        group.MapGet("/", async (ClaimsPrincipal user, ISender sender) =>
        {
            var userId = user.GetUserId();
            var result = await sender.Send(new GetAccountsQuery(userId));
            return result.ToHttpResult();
        });

        group.MapGet("/{id:guid}", async (Guid id, ClaimsPrincipal user, ISender sender) =>
        {
            var userId = user.GetUserId();
            var result = await sender.Send(new GetAccountByIdQuery(id, userId));
            return result.ToHttpResult();
        });

        group.MapPost("/", async (CreateAccountRequest request, ClaimsPrincipal user, ISender sender) =>
        {
            var userId = user.GetUserId();
            var command = new CreateAccountCommand(userId, request.Name, request.Type, request.OpeningBalance, request.OpeningDate);
            var result = await sender.Send(command);
            return result.ToCreatedResult($"/accounts/{(result.IsSuccess ? result.Value.Id : Guid.Empty)}");
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateAccountRequest request, ClaimsPrincipal user, ISender sender) =>
        {
            var userId = user.GetUserId();
            var command = new UpdateAccountCommand(id, userId, request.Name, request.Type);
            var result = await sender.Send(command);
            return result.ToHttpResult();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ClaimsPrincipal user, ISender sender) =>
        {
            var userId = user.GetUserId();
            var result = await sender.Send(new DeleteAccountCommand(id, userId));
            return result.ToHttpResult();
        });
    }

    private sealed record CreateAccountRequest(string Name, AccountType Type, decimal OpeningBalance = 0, DateOnly? OpeningDate = null);
    private sealed record UpdateAccountRequest(string Name, AccountType Type);
}
