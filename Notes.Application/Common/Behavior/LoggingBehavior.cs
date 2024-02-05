using MediatR;
using Notes.Application.Interfaces;
using Serilog;

namespace Notes.Application.Common.Behavior;

public class LoggingBehavior<TRequest, TResponse>(ICurrentUserService currentUserService)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        Log.Information("Notes Request: {Name} {@UserId} {@Request}",
            requestName, currentUserService.UserId, request);

        var response = await next();
        return response;
    }
}