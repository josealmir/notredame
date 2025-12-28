using Microsoft.Extensions.Http.Resilience;
using Notredame.Api.Settings;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace Notredame.Api.Resiliences;

public static class ResiliencyApi
{
    public static void Configure(ResilienceHandlerContext context, ResiliencePipelineBuilder<HttpResponseMessage> builder)
        {
            var logger = context
                .ServiceProvider
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger(typeof(ClientHttpNotredame));
                
            builder.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,

                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<HttpRequestException>()
                    .HandleResult(r => (int)r.StatusCode >= 500),

                OnRetry = args =>
                {
                    logger.LogWarning(
                        "Retry {RetryAttempt} After {Delay}ms. Reason: {Reason}",
                        args.AttemptNumber,
                        args.RetryDelay.TotalMilliseconds,
                        args.Outcome.Exception?.Message ??
                        $"HTTP {(int)args.Outcome.Result!.StatusCode}"
                    );

                    return ValueTask.CompletedTask;
                }
            });

            builder.AddTimeout(TimeSpan.FromSeconds(5));
            builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions<HttpResponseMessage>
            {
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                MinimumThroughput = 10,
                BreakDuration = TimeSpan.FromSeconds(20),

                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<HttpRequestException>()
                    .HandleResult(r => (int)r.StatusCode >= 500),

                OnOpened = args =>
                {
                    logger.LogError(
                        "Circuit Breaker OPEN by {Duration}s",
                        args.BreakDuration.TotalSeconds
                    );
                    return ValueTask.CompletedTask;
                },

                OnClosed = _ =>
                {
                    logger.LogInformation("Circuit Breaker CLOSE");
                    return ValueTask.CompletedTask;
                },

                OnHalfOpened = _ =>
                {
                    logger.LogInformation("Circuit Breaker state HALF-OPEN");
                    return ValueTask.CompletedTask;
                }
            });
        }
}