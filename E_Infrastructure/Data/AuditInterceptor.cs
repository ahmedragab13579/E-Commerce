using Application.Services.InterFaces.Humans;
using E_Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace E_Infrastructure.Data
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        // (الـ Constructor بتاعك زي ما هو)
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUserService;

        public AuditInterceptor(ICurrentUserService currentUserService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = currentUserService;
        }

        private readonly List<(EntityEntry Entry, AuditLog Log)> _temporaryAuditLogs = new();

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null)
                return await base.SavingChangesAsync(eventData, result, cancellationToken);

            var Id = _currentUserService.GetCurrentUserId();
            int userId;
           if (Id == null)
                userId = 0;
            userId = Id.Value;


            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in entries)
            {
                if (entry.Entity is AuditLog)
                    continue;

                if (entry.Entity is User && entry.State == EntityState.Added)
                    continue;

                int recordId = 0; 
                if (entry.State != EntityState.Added)
                {
                    var pkProperty = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());
                    if (pkProperty != null && pkProperty.CurrentValue is int pkValue)
                    {
                        recordId = pkValue;
                    }
                }

                var auditLog = new AuditLog
                {
                    TableName = entry.Metadata.GetTableName(),
                    OperationType = entry.State.ToString(),
                    ChangeDate = DateTime.UtcNow,
                    UserId = userId,
                    RecordId = recordId
                };

                context.Set<AuditLog>().Add(auditLog);

                if (entry.State == EntityState.Added)
                    _temporaryAuditLogs.Add((entry, auditLog));
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override async ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null)
                return await base.SavedChangesAsync(eventData, result, cancellationToken);

            foreach (var (entry, log) in _temporaryAuditLogs)
            {
                var pkProperty = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());
                if (pkProperty != null && pkProperty.CurrentValue is int pkValue)
                {
                    log.RecordId = pkValue;
                }
            }

            if (_temporaryAuditLogs.Any())
            {
                _temporaryAuditLogs.Clear();
                await context.SaveChangesAsync(cancellationToken);
            }

            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }
    }
}