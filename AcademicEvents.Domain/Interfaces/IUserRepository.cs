using AcademicEvents.Domain.Entities;

namespace AcademicEvents.Domain.Interfaces;

/// <summary>
/// Contrato do repository de usuarios.
/// A implementacao fica na Infrastructure.
/// </summary>
public interface IUserRepository
{
    Task<User> CreateAsync(User usuario);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetAllAsync();
    Task<User?> UpdateAsync(User usuario);
    Task DeleteAsync(int id);
}
