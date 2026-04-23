using System.Diagnostics;

namespace SteamClone.API.Middlewares
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startedAt = DateTime.Now;
            var stopwatch = Stopwatch.StartNew();
            var requestUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
            var requestInfo = $"{context.Request.Method} {requestUrl}";

            _logger.LogInformation("Request started at {StartedAt}. {RequestInfo}", startedAt, requestInfo);

            await _next(context);

            stopwatch.Stop();
            var finishedAt = DateTime.Now;
            var statusCode = context.Response.StatusCode;

            if (statusCode >= 500)
            {
                _logger.LogError(
                    "Response sent at {FinishedAt}. StatusCode: {StatusCode}. {RequestInfo}. Duration: {DurationMs} ms",
                    finishedAt,
                    statusCode,
                    requestInfo,
                    stopwatch.ElapsedMilliseconds);

                return;
            }

            if (statusCode >= 400)
            {
                _logger.LogWarning(
                    "Response sent at {FinishedAt}. StatusCode: {StatusCode}. {RequestInfo}. Duration: {DurationMs} ms",
                    finishedAt,
                    statusCode,
                    requestInfo,
                    stopwatch.ElapsedMilliseconds);

                return;
            }

            _logger.LogInformation(
                "Response sent at {FinishedAt}. StatusCode: {StatusCode}. {RequestInfo}. Duration: {DurationMs} ms",
                finishedAt,
                statusCode,
                requestInfo,
                stopwatch.ElapsedMilliseconds);
        }
    }
}
