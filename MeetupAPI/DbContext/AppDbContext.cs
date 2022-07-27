using IdentityServer.DbContext;
using MeetupAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcMeetupClient.Mocks;

namespace MeetupAPI.DbContext;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Event> Events { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> context) : base(context)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>().Property(p => p.MeetupName).IsRequired();
        modelBuilder.Entity<Event>().Property(p => p.Theme).IsRequired();
        modelBuilder.Entity<Event>().Property(p => p.Description).IsRequired(false);
        modelBuilder.Entity<Event>().Property(p => p.Schedule).IsRequired(false);
        modelBuilder.Entity<Event>().Property(p => p.Orgonizer).IsRequired();
        modelBuilder.Entity<Event>().Property(p => p.Speeker).IsRequired(false);
        modelBuilder.Entity<Event>().Property(p => p.Time).IsRequired();
        modelBuilder.Entity<Event>().Property(p => p.Location).IsRequired();
        
        modelBuilder.Entity<Event>().HasData(EventsMock.Get());
    }
}