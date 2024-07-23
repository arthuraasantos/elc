using Core.Entities.Common;
using Infra.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;

namespace Infra
{
    public class UnitofWork : IUnitofWork
    {
        private readonly TMContext _tmContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string ActionUser { get { return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) ?? string.Empty; } }
        public Guid ActionUserId { get { 
                return Guid.Parse(_httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value?.ToString()); 
            } }

        public UnitofWork(TMContext context, IHttpContextAccessor httpContextAccessor)
        {
            _tmContext = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> CommitAsync()
        {
            string actionUser = ActionUser;
            Guid actionUserId = ActionUserId;

            var entries = _tmContext.ChangeTracker.Entries()
               .Where(a => a.Entity is AuditEntity)
               .Where(a => a.State == EntityState.Added || a.State == EntityState.Modified || a.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                var auditableDefaultEntity = entry.Entity as AuditEntity;

                if (auditableDefaultEntity == null)
                    continue;

                if (entry.State == EntityState.Added)
                {
                    auditableDefaultEntity.CreatedAt = DateTime.UtcNow;
                    auditableDefaultEntity.CreatedUser = actionUser;
                    auditableDefaultEntity.CreatedUserId = actionUserId;
                }

                if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
                {
                    auditableDefaultEntity.UpdatedAt = DateTime.UtcNow;
                    auditableDefaultEntity.UpdatedUser = actionUser;
                    auditableDefaultEntity.UpdatedUserId = actionUserId;
                }

                if (entry.State == EntityState.Deleted)
                {
                    auditableDefaultEntity.DeletedAt = DateTime.UtcNow;
                    auditableDefaultEntity.DeleteUser = actionUser;
                    auditableDefaultEntity.DeleteUserId = actionUserId;
                    entry.State = EntityState.Modified;
                }
            }

            return await Task.FromResult(_tmContext.SaveChanges());
        }
    }
}
