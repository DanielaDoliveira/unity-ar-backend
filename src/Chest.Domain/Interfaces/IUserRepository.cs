using Chest.Domain.Entities;

namespace Chest.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId);
    Task<User?> GetByNameAsync(string name); 
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid userId);
}