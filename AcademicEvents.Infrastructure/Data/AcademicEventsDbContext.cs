using AcademicEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcademicEvents.Infrastructure.Data;

/// <summary>
/// DbContext principal do projeto.
/// Mapeia as entidades do dominio para o PostgreSQL via EF Core.
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // email unico por usuario
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        // um usuario nao pode se inscrever duas vezes no mesmo evento
        modelBuilder.Entity<Registration>()
            .HasIndex(r => new { r.UsuarioId, r.EventoId })
            .IsUnique();

        // relacionamento de Event com User (organizador)
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Organizador)
            .WithMany(u => u.EventosOrganizados)
            .HasForeignKey(e => e.OrganizadorId)
            .OnDelete(DeleteBehavior.Restrict);

        // relacionamento de Registration com User
        modelBuilder.Entity<Registration>()
            .HasOne(r => r.Usuario)
            .WithMany(u => u.Inscricoes)
            .HasForeignKey(r => r.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // relacionamento de Registration com Event
        modelBuilder.Entity<Registration>()
            .HasOne(r => r.Evento)
            .WithMany(e => e.Inscricoes)
            .HasForeignKey(r => r.EventoId)
            .OnDelete(DeleteBehavior.Cascade);

        // relacionamento de Comment com User
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Usuario)
            .WithMany(u => u.Comentarios)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // relacionamento de Comment com Event
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Evento)
            .WithMany(e => e.Comentarios)
            .HasForeignKey(c => c.EventoId)
            .OnDelete(DeleteBehavior.Cascade);

        // relacionamento de Reaction com User
        modelBuilder.Entity<Reaction>()
            .HasOne(r => r.Usuario)
            .WithMany(u => u.Reacoes)
            .HasForeignKey(r => r.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // relacionamento de Reaction com Event
        modelBuilder.Entity<Reaction>()
            .HasOne(r => r.Evento)
            .WithMany(e => e.Reacoes)
            .HasForeignKey(r => r.EventoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
