using IdentityServer.DbContext;
using MeetupAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeetupAPI.DbContext;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Theme> Themes { get; set; }
    public DbSet<Event> Events { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> context) : base(context)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>().Property(p => p.Description).IsRequired(false);
        modelBuilder.Entity<Event>().Property(p => p.Schedule).IsRequired(false);
        modelBuilder.Entity<Event>().Property(p => p.SpeekerName).IsRequired(false);

        modelBuilder.Entity<Event>().HasOne(s => s.Theme).WithMany(d => d.Events);

        modelBuilder.Entity<Theme>()
            .HasAlternateKey(x => x.Value);
    }
}