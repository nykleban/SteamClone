using SteamClone.BLL.Services;

namespace SteamClone.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var requestUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";

                _logger.LogError(
                    ex,
                    "Unhandled exception while processing {Method} {RequestUrl}",
                    context.Request.Method,
                    requestUrl);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = ServiceResponse.Error(ex.Message);

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
