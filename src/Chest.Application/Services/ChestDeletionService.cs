using Chest.Application.Interface;
using Chest.Domain.Interfaces;

namespace Chest.Application.Services;

public class ChestDeletionService : IChestDeletionService
{
    private readonly IChestRepository _repository;

    public ChestDeletionService(IChestRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> DeleteChestAsync(Guid chestId, Guid userId)
    {
        var chest = await _repository.GetByIdAsync(chestId);

        if (chest == null) return false;

        if (chest.UserId != userId)
            throw new UnauthorizedAccessException("You are not the owner of this chest.");
        

        await _repository.DeleteAsync(chest);
        return true;
    }
}