using Chest.Domain.Interfaces;
using Chest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Chest.Infrastructure.Repository;

public class ChestRepository : IChestRepository
{
    private readonly ChestDbContext _context;

    public ChestRepository(ChestDbContext context) => _context = context;

    public async Task AddAsync(Domain.Entities.Chest chest)
    {
        await _context.Chests.AddAsync(chest);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Domain.Entities.Chest>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Chests
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task<Domain.Entities.Chest?> GetByIdAsync(Guid id)
    {
        // O FindAsync é ideal para chaves primárias
        return await _context.Chests.FindAsync(id);
    }

    public async Task<bool> DeleteAsync(Domain.Entities.Chest chest)
    {
        _context.Chests.Remove(chest);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Domain.Entities.Chest>> GetAvailableChestsAsync()
    {
        return await _context.Chests
            .Where(c => !c.IsSomebodyPlaying)
            .ToListAsync();
    }
    public async Task UpdateAsync(Domain.Entities.Chest chest)
    {
        _context.Chests.Update(chest);
        await _context.SaveChangesAsync();
    }
}