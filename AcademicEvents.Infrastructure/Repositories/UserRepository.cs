using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicEvents.Infrastructure.Repositories;

/// <summary>
/// Repository de usuarios. Usa o DbContext para acessar o PostgreSQL.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly AcademicEventsDbContext _context;

    public UserRepository(AcademicEventsDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateAsync(User usuario)
    {
        _context.Users.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> UpdateAsync(User usuario)
    {
        _context.Users.Update(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task DeleteAsync(int id)
    {
        User? usuario = await _context.Users.FindAsync(id);
        if (usuario is null) return;
        _context.Users.Remove(usuario);
        await _context.SaveChangesAsync();
    }
}
