using Data.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Middleware
{
    public class RoleValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out var userId))
                {
                    var db = context.RequestServices.GetRequiredService<EduPlusDbContext>();

                    var currentRoleLevels = await db.UserRoles
                        .Where(ur => ur.UserId == userId)
                        .Join(db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Level)
                        .OrderBy(l => l)
                        .ToListAsync();

                    var tokenRoleLevels = context.User.FindFirst("roleLevels")?.Value ?? "";
                    var tokenLevels = tokenRoleLevels
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .OrderBy(l => l)
                        .ToList();

                    if (!currentRoleLevels.SequenceEqual(tokenLevels))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync("{\"error\":\"Twoje uprawnienia uległy zmianie. Zaloguj się ponownie.\"}");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
