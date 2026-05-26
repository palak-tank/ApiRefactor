using System.Diagnostics;
using MediatR;

namespace ApiRefactor.Features.Common;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) =>
        _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Handling {RequestName} {@Request}", requestName, request);

        var stopwatch = Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();

        var elapsed = stopwatch.ElapsedMilliseconds;

        if (elapsed > 500)
            _logger.LogWarning(
                "Slow handler detected: {RequestName} completed in {ElapsedMs}ms",
                requestName, elapsed);
        else
            _logger.LogInformation(
                "Handled {RequestName} in {ElapsedMs}ms",
                requestName, elapsed);

        return response;
    }
}
