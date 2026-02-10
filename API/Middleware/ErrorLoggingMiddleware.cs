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
                var userIdClaim = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                int? userId = int.TryParse(userIdClaim, out var uid) ? uid : null;
                var endpoint = $"{context.Request.Method} {context.Request.Path}";

                _eventLogService.Log("ERROR", userId: userId, reason: endpoint, ex: ex);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Wystąpił błąd serwera" }));
            }
        }
    }
}
