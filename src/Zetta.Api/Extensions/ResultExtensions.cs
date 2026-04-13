using Zetta.SharedKernel.Results;

namespace Zetta.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return MapError(result.Error);
    }

    public static IResult ToCreatedResult<T>(this Result<T> result, string location)
    {
        if (result.IsSuccess)
            return Results.Created(location, result.Value);

        return MapError(result.Error);
    }

    public static IResult ToHttpResult(this Result result)
    {
        if (result.IsSuccess)
            return Results.NoContent();

        return MapError(result.Error);
    }

    private static IResult MapError(Error error) => error.Code switch
    {
        "Auth.Unauthorized" => Results.Unauthorized(),
        var c when c.EndsWith(".NotFound") => Results.NotFound(new { error.Code, error.Message }),
        var c when c.EndsWith(".Conflict") => Results.Conflict(new { error.Code, error.Message }),
        "Validation.Error" => Results.UnprocessableEntity(new { error.Code, error.Message }),
        _ => Results.BadRequest(new { error.Code, error.Message })
    };
}
