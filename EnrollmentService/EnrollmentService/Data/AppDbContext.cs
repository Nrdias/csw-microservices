using Microsoft.EntityFrameworkCore;
using EnrollmentService.Models;

namespace EnrollmentService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Enrollment> Enrollments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Enrollment>()
            .HasIndex(e => new { e.StudentRegistrationNumber, e.DisciplineCode, e.Schedule })
            .IsUnique();
    }
}