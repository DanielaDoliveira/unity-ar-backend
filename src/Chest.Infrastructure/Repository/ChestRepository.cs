using Chest.Domain.Interfaces;
using Chest.Infrastructure.Data;

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

    public async Task<Domain.Entities.Chest?> GetByIdAsync(Guid id)
    {
        return await _context.Chests.FindAsync(id);
    }  
}