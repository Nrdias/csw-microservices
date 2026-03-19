using Microsoft.EntityFrameworkCore;
using DisciplineService.Models;

namespace DisciplineService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Discipline> Disciplines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Discipline>()
            .HasIndex(d => new { d.Code, d.Schedule })
            .IsUnique();
    }
}