using FluentValidation;
using MediatR;

namespace Notes.Application.Common.Behavior;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<IRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var ctx = new ValidationContext<TRequest>(request);
        var failures = validators
            .Select(v => v.Validate(ctx))
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .ToList();

        if (failures.Count != 0) throw new ValidationException(failures);

        return next();
    }
}