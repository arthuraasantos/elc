using Core.Entities.Common;
using Core.Entities.Files;
using Core.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Contexts
{
    public class TMContext: IdentityDbContext<AppUser,IdentityRole<Guid>, Guid>
    {
        public TMContext(DbContextOptions<TMContext> options) : base(options) { }

        public virtual DbSet<MyFile> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MyFile>().HasKey(a => a.Id);
            builder.Entity<MyFile>().HasOne(a => a.Owner).WithMany().HasForeignKey(fk => fk.OwnerId);

            builder.Entity<MyFile>().ToTable("Files");

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            string actionUser = "user";
            Guid actionUserId = Guid.NewGuid();

            var entries = ChangeTracker.Entries()
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
                    auditableDefaultEntity.UpdatedUserId= actionUserId;
                }

                if (entry.State == EntityState.Deleted)
                {
                    auditableDefaultEntity.DeletedAt = DateTime.UtcNow;
                    auditableDefaultEntity.DeleteUser = actionUser;
                    auditableDefaultEntity.DeleteUserId = actionUserId;
                }
            }
            
            return base.SaveChanges();
        }
    }
}
