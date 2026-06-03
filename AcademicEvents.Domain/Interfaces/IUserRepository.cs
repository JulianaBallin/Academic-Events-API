using AcademicEvents.Domain.Entities;

namespace AcademicEvents.Domain.Interfaces;

/// <summary>
/// User repository contract.
/// Implementation kept on Infrastructure.
/// </summary>
public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetAllAsync();
    Task<User?> UpdateAsync(User user);
    Task DeleteAsync(int id);
}
