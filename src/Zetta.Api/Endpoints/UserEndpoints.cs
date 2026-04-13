using System.Security.Claims;
using MediatR;
using Zetta.Api.Extensions;
using Zetta.Application.Users.Commands.ChangePassword;
using Zetta.Application.Users.Commands.UpdateUser;
using Zetta.Application.Users.Queries.GetUserById;

namespace Zetta.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users/me").WithTags("Users").RequireAuthorization();

        group.MapGet("/", async (ClaimsPrincipal user, ISender sender) =>
        {
            var userId = user.GetUserId();
            var result = await sender.Send(new GetUserByIdQuery(userId));
            return result.ToHttpResult();
        });

        group.MapPut("/", async (UpdateUserRequest request, ClaimsPrincipal user, ISender sender) =>
        {
            var userId = user.GetUserId();
            var result = await sender.Send(new UpdateUserCommand(userId, request.Name));
            return result.ToHttpResult();
        });

        group.MapPost("/change-password", async (ChangePasswordRequest request, ClaimsPrincipal user, ISender sender) =>
        {
            var userId = user.GetUserId();
            var result = await sender.Send(new ChangePasswordCommand(userId, request.CurrentPassword, request.NewPassword));
            return result.ToHttpResult();
        });
    }

    private sealed record UpdateUserRequest(string Name);
    private sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);
}
