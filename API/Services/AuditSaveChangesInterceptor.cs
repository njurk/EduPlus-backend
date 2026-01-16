using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;

namespace API
{
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditSaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            SetAuditFields(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            SetAuditFields(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void SetAuditFields(DbContext? context)
        {
            if (context == null) return;

            var userId = GetCurrentUserId();
            var now = DateTime.Now;

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    SetPropertyIfExists(entry, "CreatedAt", now);
                    SetPropertyIfExists(entry, "UpdatedAt", now);
                    SetPropertyIfExists(entry, "ModifiedByUserId", userId);
                }
                else if (entry.State == EntityState.Modified)
                {
                    SetPropertyIfExists(entry, "UpdatedAt", now);
                    SetPropertyIfExists(entry, "ModifiedByUserId", userId);
                }
            }
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }

        private static void SetPropertyIfExists(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry, string propertyName, object? value)
        {
            var property = entry.Properties.FirstOrDefault(p => p.Metadata.Name == propertyName);
            if (property != null)
            {
                property.CurrentValue = value;
            }
        }
    }
}
