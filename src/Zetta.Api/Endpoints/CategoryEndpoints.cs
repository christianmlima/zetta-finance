using System.Security.Claims;
using MediatR;
using Zetta.Api.Extensions;
using Zetta.Application.Categories.Commands.CreateCategory;
using Zetta.Application.Categories.Commands.DeleteCategory;
using Zetta.Application.Categories.Commands.UpdateCategory;
using Zetta.Application.Categories.Queries.GetCategories;
using Zetta.Domain.Enums;

namespace Zetta.Api.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/categories").WithTags("Categories").RequireAuthorization();

        group.MapGet("/", async (ClaimsPrincipal user, ISender sender) =>
        {
            var userId = user.GetUserId();
            var result = await sender.Send(new GetCategoriesQuery(userId));
            return result.ToHttpResult();
        });

        group.MapPost("/", async (CreateCategoryRequest request, ClaimsPrincipal user, ISender sender) =>
        {
            var userId = user.GetUserId();
            var command = new CreateCategoryCommand(userId, request.Name, request.Type, request.Icon, request.Color, request.ParentCategoryId);
            var result = await sender.Send(command);
            return result.ToCreatedResult($"/categories/{(result.IsSuccess ? result.Value.Id : Guid.Empty)}");
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateCategoryRequest request, ClaimsPrincipal user, ISender sender) =>
        {
            var userId = user.GetUserId();
            var command = new UpdateCategoryCommand(id, userId, request.Name, request.Icon, request.Color);
            var result = await sender.Send(command);
            return result.ToHttpResult();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ClaimsPrincipal user, ISender sender) =>
        {
            var userId = user.GetUserId();
            var result = await sender.Send(new DeleteCategoryCommand(id, userId));
            return result.ToHttpResult();
        });
    }

    private sealed record CreateCategoryRequest(
        string Name,
        CategoryType Type,
        string Icon = "tag",
        string Color = "#6366F1",
        Guid? ParentCategoryId = null);

    private sealed record UpdateCategoryRequest(string Name, string Icon, string Color);
}
