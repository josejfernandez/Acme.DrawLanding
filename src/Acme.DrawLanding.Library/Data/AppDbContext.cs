using Acme.DrawLanding.Library.Domain.SerialNumbers;
using Acme.DrawLanding.Library.Domain.Submissions;
using Acme.DrawLanding.Library.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Acme.DrawLanding.Library.Data;

public class AppDbContext : DbContext
{
    public DbSet<SerialNumberRecord> SerialNumbers { get; set; }
    public DbSet<SubmissionRecord> Submissions { get; set; }
    public DbSet<UserRecord> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SerialNumberRecord>()
            .ToTable("SerialNumbers")
            // Conversion necessary, otherwise WHERE with Guids does not work well in SQLite.
            .Property(x => x.Content).HasConversion(new GuidToStringConverter());

        modelBuilder.Entity<SubmissionRecord>()
            .ToTable("Submissions");

        modelBuilder.Entity<UserRecord>()
            .ToTable("Users");
    }
}
