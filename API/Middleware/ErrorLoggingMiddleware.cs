using BusinessLogic.Services;
using System.Net;
using System.Text.Json;

namespace API.Middleware
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEventLogService _eventLogService;

        public ErrorLoggingMiddleware(RequestDelegate next, IEventLogService eventLogService)
        {
            _next = next;
            _eventLogService = eventLogService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _eventLogService.LogError(
                    $"{context.Request.Method} {context.Request.Path}",
                    ex.Message,
                    ex
                );

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = JsonSerializer.Serialize(new { error = "Wystąpił błąd serwera" });
                await context.Response.WriteAsync(response);
            }
        }
    }
}
