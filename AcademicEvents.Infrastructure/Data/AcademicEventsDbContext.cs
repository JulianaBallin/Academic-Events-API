using AcademicEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcademicEvents.Infrastructure.Data;

/// <summary>
/// Main DbContext for the project.
/// Maps domain entities to PostgreSQL using EF Core.
/// </summary>
public class AcademicEventsDbContext : DbContext
{
    public AcademicEventsDbContext(DbContextOptions<AcademicEventsDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Registration> Registrations => Set<Registration>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Reaction> Reactions => Set<Reaction>();
    public DbSet<Activity> Activities => Set<Activity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Email must be unique per user.
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        // A user cannot register for the same event twice.
        modelBuilder.Entity<Registration>()
            .HasIndex(r => new { r.UserId, r.EventId })
            .IsUnique();

        // Relationship between Event and User (organizer).
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Organizer)
            .WithMany(u => u.OrganizedEventList)
            .HasForeignKey(e => e.OrganizerId)
            .OnDelete(DeleteBehavior.Restrict);

        //  Relationship between Registration and User.
        modelBuilder.Entity<Registration>()
            .HasOne(r => r.User)
            .WithMany(u => u.Registrations)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship between Registration and Event.
        modelBuilder.Entity<Registration>()
            .HasOne(r => r.Event)
            .WithMany(e => e.Registrations)
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship between Comment and User.
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        //  Relationship between Comment and Event.
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Event)
            .WithMany(e => e.Comments)
            .HasForeignKey(c => c.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        //  Relationship between Reaction and User.
        modelBuilder.Entity<Reaction>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reactions)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        //  Relationship between Reaction and Event.
        modelBuilder.Entity<Reaction>()
            .HasOne(r => r.Event)
            .WithMany(e => e.Reactions)
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // A user cannot react to the same event twice.
        modelBuilder.Entity<Reaction>()
            .HasIndex(r => new { r.UserId, r.EventId })
            .IsUnique();

        // Relationship between Activity and Event.
        modelBuilder.Entity<Activity>()
        .HasOne(a => a.Event)
        .WithMany(e => e.Activities)
        .HasForeignKey(a => a.EventId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
