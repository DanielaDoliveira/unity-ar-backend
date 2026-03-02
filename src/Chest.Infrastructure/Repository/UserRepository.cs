using Chest.Domain.Entities;
using Chest.Domain.Interfaces;
using Chest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Chest.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ChestDbContext _context;

    public UserRepository(ChestDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<User?> GetByNameAsync(string name)
    {
        // Usamos ToLower() para evitar problemas de maiúsculas/minúsculas no login
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}