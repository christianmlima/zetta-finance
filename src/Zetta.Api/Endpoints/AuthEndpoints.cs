using MediatR;
using Zetta.Api.Extensions;
using Zetta.Application.Users.Commands.Login;
using Zetta.Application.Users.Commands.RegisterUser;

namespace Zetta.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("/register", async (RegisterRequest request, ISender sender) =>
        {
            var command = new RegisterUserCommand(request.Name, request.Email, request.Password);
            var result = await sender.Send(command);
            return result.ToCreatedResult($"/users/me");
        });

        group.MapPost("/login", async (LoginRequest request, ISender sender) =>
        {
            var command = new LoginCommand(request.Email, request.Password);
            var result = await sender.Send(command);
            return result.ToHttpResult();
        });
    }

    private sealed record RegisterRequest(string Name, string Email, string Password);
    private sealed record LoginRequest(string Email, string Password);
}
