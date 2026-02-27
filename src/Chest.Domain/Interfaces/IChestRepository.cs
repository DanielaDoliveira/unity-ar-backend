namespace Chest.Domain.Interfaces;

public interface IChestRepository
{
    // Adiciona um novo baú ao banco
    Task AddAsync(Chest.Domain.Entities.Chest chest);
    Task<IEnumerable<Chest.Domain.Entities.Chest>> GetByUserIdAsync(Guid userId);
    Task<Domain.Entities.Chest?> GetByIdAsync(Guid id);
    Task<bool> DeleteAsync(Domain.Entities.Chest chest);
    Task<IEnumerable<Domain.Entities.Chest>> GetAvailableChestsAsync();
    
    Task UpdateAsync(Domain.Entities.Chest chest);
}