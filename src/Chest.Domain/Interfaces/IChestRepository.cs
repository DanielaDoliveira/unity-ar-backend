namespace Chest.Domain.Interfaces;

public interface IChestRepository
{
    // Adiciona um novo baú ao banco
    Task AddAsync(Chest.Domain.Entities.Chest chest);
}