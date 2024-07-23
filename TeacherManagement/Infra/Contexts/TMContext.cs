using Core.Entities.Files;
using Core.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Contexts
{
    public class TMContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        
        public TMContext(DbContextOptions<TMContext> options) : base(options)
        {
        }

        public virtual DbSet<MyFile> Files { get; set; }
        public virtual DbSet<MyFileData> FilesData { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MyFile>().HasKey(a => a.Id);
            builder.Entity<MyFile>().HasOne(a => a.Owner).WithMany().HasForeignKey(fk => fk.OwnerId);
            builder.Entity<MyFile>().ToTable("Files");

            builder.Entity<MyFileData>().HasKey(fd => fd.Id);
            builder.Entity<MyFileData>().Property(fd => fd.ExtractedData).HasColumnType("varchar(max)").HasMaxLength(-1);
            builder.Entity<MyFileData>().HasOne(fd => fd.File).WithMany().HasForeignKey(fk => fk.FileId);
            builder.Entity<MyFileData>().ToTable("FilesData");

            base.OnModelCreating(builder);
        }
    }
}
