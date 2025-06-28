using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Web.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly int timePerformanceWarningInMilliseconds = 1000;

    public PerformanceBehaviour(IConfiguration configuration, ILogger<TRequest> logger)
    {
        _timer = new Stopwatch();
        _logger = logger;
        if(configuration != null) {
            timePerformanceWarningInMilliseconds = configuration.GetValue<int>("MediatorSettings:TimePerformanceWarningInMilliseconds");
        }
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();
        var response = await next();
        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;
        if (elapsedMilliseconds > timePerformanceWarningInMilliseconds)
        {
            _logger.LogWarning("Long Running Request: {ElapsedMilliseconds} milliseconds", elapsedMilliseconds);
        }

        return response;
    }
}
