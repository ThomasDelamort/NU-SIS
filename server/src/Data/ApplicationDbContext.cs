using Microsoft.EntityFrameworkCore;
using server.src.Models;

namespace server.src.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }

    public DbSet<DailyLog> DailyLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("students");

            entity.HasKey(s => s.StudentId);

            entity.Property(s => s.RfidUid)
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(s => s.RfidUid)
                .IsUnique();

            entity.Property(s => s.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(s => s.LastName)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(s => s.UniversityId)
                .HasMaxLength(20)
                .IsRequired();

            entity.HasIndex(s => s.UniversityId)
                .IsUnique();

            entity.Property(s => s.UniversityEmail)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(s => s.ContactNumber)
                .HasMaxLength(20);

            entity.Property(s => s.ProfilePicture)
                .HasMaxLength(255);

            entity.Property(s => s.EmergencyContactNumber)
                .HasMaxLength(20);

            entity.Property(s => s.CreatedAt)
                .IsRequired();
        });

        modelBuilder.Entity<DailyLog>(entity =>
        {
            entity.ToTable("daily_logs");

            entity.HasKey(log => log.LogId);

            entity.Property(log => log.TimeIn)
                .IsRequired();

            entity.HasOne(log => log.Student)
                .WithMany(student => student.DailyLogs)
                .HasForeignKey(log => log.StudentId);
        });
    }
}