using System.Collections.Concurrent;
using System.Reflection;
using FluentValidation;
using MediatR;
using Zetta.SharedKernel.Results;

namespace Zetta.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private static readonly ConcurrentDictionary<Type, Func<Error, object>> _failureFactory = new();

    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count == 0)
            return await next();

        var message = string.Join("; ", failures.Select(f => f.ErrorMessage));
        var error = Error.Validation(message);

        var factory = _failureFactory.GetOrAdd(typeof(TResponse), BuildFailureFactory);
        return (TResponse)factory(error);
    }

    private static Func<Error, object> BuildFailureFactory(Type responseType)
    {
        if (responseType == typeof(Result))
            return error => Result.Failure(error);

        var valueType = responseType.GetGenericArguments()[0];
        var method = typeof(Result)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .First(m => m.Name == nameof(Result.Failure) && m.IsGenericMethod)
            .MakeGenericMethod(valueType);

        return error => method.Invoke(null, [error])!;
    }
}
