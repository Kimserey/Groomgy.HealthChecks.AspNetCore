using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Net;
using System.Net.Http;

namespace Groomgy.HealthChecks.AspNetCore.Implementations
{
    public static class PolicyHandler
    {
        /// <summary>
        /// Handle 5.x.x, 408 timeout, 404 not found and timeout rejection from client and retry for the retry count specified.
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> WaitAndRetry(Func<int, TimeSpan> sleepDurationProvider, Action<DelegateResult<HttpResponseMessage>, TimeSpan> onFailure, int retryCount = 5) =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(retryCount, sleepDurationProvider, onFailure);

        /// <summary>
        /// Handle 5.x.x, 408 timeout, 404 not found and timeout rejection from client and retry 5 times while sleeping in an exponential manner.
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> WaitAndRetry(int retryCount = 5) =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        public static IAsyncPolicy<HttpResponseMessage> Timeout(int seconds = 2) =>
            Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(seconds));
    }
}
